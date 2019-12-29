using FRTools.Data;
using FRTools.Data.DataModels;
using FRTools.Data.DataModels.DiscordModels;
using FRTools.Web.Infrastructure;
using FRTools.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("discord")]
    public class DiscordController : BaseController
    {
        private static DiscordMetadata DiscordMetadata { get; }
        private static long DiscordBotOwnerId { get; }

        static DiscordController()
        {
            var json = System.IO.File.ReadAllText("DiscordMetadata.json");
            DiscordMetadata = JsonConvert.DeserializeObject<DiscordMetadata>(json);

            if (long.TryParse(ConfigurationManager.AppSettings["DiscordBotOwner"], out var id))
                DiscordBotOwnerId = id;
        }

        private long? CurrentUserId { get; }

        public DiscordController()
        {
            if (Request.IsAuthenticated)
            {
                CurrentUserId = Session["CurrentDiscordUser"] as long?;
                if (CurrentUserId == null)
                {
                    var owin = HttpContext.GetOwinContext();
                    var userManager = owin.GetUserManager<UserManager<User, int>>();
                    var logins = userManager.GetLogins(HttpContext.User.Identity.GetUserId<int>());
                    var discordLogin = logins.FirstOrDefault(x => x.LoginProvider.ToLower() == "discord");
                    if (discordLogin != null)
                        Session["CurrentDiscordUser"] = CurrentUserId = long.Parse(discordLogin.ProviderKey);
                }
            }
        }

        [Route(Name = "DiscordHome")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("help", Name = "DiscordHelp")]
        public ActionResult Help()
        {
            return View();
        }

        [Route("manage", Name = "DiscordManage")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult Manage()
        {
            using (var ctx = new DataContext())
            {
                var currentUser = ctx.DiscordUsers.First(x => x.UserId == CurrentUserId);
                return View(new ServersViewModel
                {
                    Servers = currentUser.Servers.Select(x => x.Server).ToList(),
                    CurrentUser = currentUser
                });
            }
        }

        [Route("manage/{discordServer}", Name = "DiscordManageServer")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult Manage(long discordServer)
        {
            using (var ctx = new DataContext())
            {
                var currentUser = ctx.DiscordUsers.First(x => x.UserId == CurrentUserId);

                if (CheckMutualServer(discordServer, currentUser))
                {
                    var model = new ModulesViewModel
                    {
                        CurrentUser = currentUser,
                        SelectedServer = discordServer,
                        Modules = DiscordMetadata.Modules
                    };
                    return View(model);
                }
                else
                    return RedirectToRoute("DiscordManage");
            }
        }

        [Route("manage/{discordServer}/{module}", Name = "DiscordManageModule")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult ManageModule(long discordServer, string module)
        {
            using (var ctx = new DataContext())
            {
                var currentUser = ctx.DiscordUsers.First(x => x.UserId == CurrentUserId);

                if (CheckMutualServer(discordServer, currentUser))
                {
                    if (CheckModule(module))
                    {
                        var model = new ModuleViewModel
                        {
                            CurrentUser = currentUser,
                            SelectedServer = discordServer,
                            SelectedModule = DiscordMetadata.Modules.First(x => x.Name.ToLower() == module)
                        };
                        return View(model);
                    }
                    else
                    {
                        TempData["Error"] = $"Could not find module '{module}', or you do not have access to it on this server";
                        return RedirectToRoute("DiscordManageServer", new { discordServer });
                    }
                }
                else
                    return RedirectToRoute("DiscordManage");
            }
        }

        [Route("manage/{discordServer}/{module}/{command}", Name = "DiscordManageCommand")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult ManageCommand(long discordServer, string module, string command)
        {
            using (var ctx = new DataContext())
            {
                var currentUser = ctx.DiscordUsers.First(x => x.UserId == CurrentUserId);

                if (CheckMutualServer(discordServer, currentUser))
                {
                    if (CheckCommand(module, command, currentUser.Servers.First(x => x.Server.ServerId == discordServer)))
                    {
                        var model = new CommandViewModel
                        {
                            CurrentUser = currentUser,
                            SelectedServer = discordServer,
                            SelectedModule = DiscordMetadata.Modules.First(x => x.Name.ToLower() == module)
                        };
                        model.SelectedCommand = model.SelectedModule.Commands.First(x => x.Name.ToLower() == command);
                        return View(model);
                    }
                    else
                    {
                        TempData["Error"] = $"Could not find command '{command}', or you do not have access to it on this server";
                        return RedirectToRoute("DiscordManageModule", new { discordServer, module });
                    }
                }
                else
                    return RedirectToRoute("DiscordManage");
            }
        }

        [Route("save", Name = "DiscordSaveSetting")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        [HttpPost]
        public ActionResult SaveSetting(long discordServer, string module, string key, string value)
        {
            ActionResult ErrorResult() => new HttpUnauthorizedResult("User does not have required permission");
            using (var ctx = new DataContext())
            {
                var currentUser = ctx.DiscordUsers.FirstOrDefault(x => x.UserId == CurrentUserId)?.Servers.FirstOrDefault(x => x.Server.ServerId == discordServer);
                if (currentUser == null)
                    return ErrorResult();

                Data.DataModels.DiscordModels.DiscordSetting setting = null;
                if (module == null)
                {
                    var botSetting = DiscordMetadata.BotSettings.FirstOrDefault(x => x.Key.ToLower() == key.ToLower());
                    if (botSetting == null)
                        return ErrorResult();

                    if (currentUser.Roles.Any(x => (x.DiscordPermissions & 8) != 0))
                    {
                        setting = ctx.DiscordSettings.FirstOrDefault(x => x.Server == currentUser.Server && x.Key == botSetting.Key);
                        if (setting == null)
                            setting = ctx.DiscordSettings.Add(new Data.DataModels.DiscordModels.DiscordSetting { Server = currentUser.Server, Key = botSetting.Key });
                        setting.Value = value;
                    }
                }
                else
                {
                    var discordModule = DiscordMetadata.Modules.FirstOrDefault(x => x.Name.ToLower() == module.ToLower());
                    if (discordModule == null)
                        return ErrorResult();

                    var moduleSetting = discordModule.Settings.FirstOrDefault(x => x.Key.ToLower() == key.ToLower());
                    if (moduleSetting == null)
                        return ErrorResult();

                    if (currentUser.Roles.Any(x => (x.DiscordPermissions & 8) != 0))
                    {
                        setting = ctx.DiscordSettings.FirstOrDefault(x => x.Server == currentUser.Server && x.Key == moduleSetting.Key);
                        if (setting == null)
                            setting = ctx.DiscordSettings.Add(new Data.DataModels.DiscordModels.DiscordSetting { Server = currentUser.Server, Key = moduleSetting.Key });
                        setting.Value = value;
                    }
                }

                ctx.SaveChanges();
                return Json(JsonConvert.SerializeObject(new { result = 1, setting }));
            }
        }

        private bool CheckMutualServer(long discordServer, DiscordUser currentUser)
        {
            if (currentUser.Servers.Select(x => x.Server).Any(x => x.ServerId == discordServer))
                return true;
            else
            {
                TempData["Error"] = $"Could not find server id '{discordServer}' in the list of mutual servers between you and the bot";
                return false;
            }
        }

        private bool CheckModule(string module)
        {
            var discordModule = DiscordMetadata.Modules.FirstOrDefault(x => x.Name.ToLower() == module);
            if (discordModule == null)
                return false;

            if (discordModule.RequireOwner && CurrentUserId != DiscordBotOwnerId)
                return false;

            return true;
        }

        private bool CheckCommand(string module, string command, DiscordServerUser currentUser)
        {
            if (CheckModule(module))
            {
                var discordCommand = DiscordMetadata.Modules.Where(x => x.Name.ToLower() == module).SelectMany(x => x.Commands).FirstOrDefault(x => x.Name.ToLower() == command);

                if (discordCommand == null)
                    return false;

                if (discordCommand.RequireOwner && CurrentUserId != DiscordBotOwnerId)
                    return false;

                if (currentUser.Roles.Any(x => (x.DiscordPermissions & discordCommand.RequireGuildPermission) != 0))
                    return true;
            }
            return false;
        }
    }
}