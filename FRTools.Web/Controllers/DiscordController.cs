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
using System.Web.Hosting;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("discord")]
    public class DiscordController : BaseController
    {
        private static DiscordMetadata DiscordMetadata { get; }
        private static long DiscordBotOwnerId { get; }

        static DiscordController()
        {
            var json = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/bin/DiscordMetadata.json"));
            DiscordMetadata = JsonConvert.DeserializeObject<DiscordMetadata>(json);

            if (long.TryParse(ConfigurationManager.AppSettings["DiscordBotOwner"], out var id))
                DiscordBotOwnerId = id;
        }

        private long? _currentUserId;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.IsAuthenticated)
            {
                _currentUserId = Session["CurrentDiscordUser"] as long?;
                if (_currentUserId == null)
                {
                    var owin = HttpContext.GetOwinContext();
                    var userManager = owin.GetUserManager<UserManager<User, int>>();
                    var logins = userManager.GetLogins(HttpContext.User.Identity.GetUserId<int>());
                    var discordLogin = logins.FirstOrDefault(x => x.LoginProvider.ToLower() == "discord");
                    if (discordLogin != null)
                        Session["CurrentDiscordUser"] = _currentUserId = long.Parse(discordLogin.ProviderKey);
                }
            }

            base.OnActionExecuting(filterContext);
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
                var currentUser = ctx.DiscordUsers.FirstOrDefault(x => x.UserId == _currentUserId);
                if (currentUser == null)
                    TempData["Warning"] = "The bot has not encountered you at all yet in any servers, are you in a mutual server with the bot?";
                return View(new ServersViewModel
                {
                    Servers = currentUser?.Servers.Select(x => new ServerViewModel
                    {
                        ServerId = x.Server.ServerId,
                        UserCount = x.Server.Users.Count,
                        IconBase64 = x.Server.IconBase64,
                        ServerName = x.Server.Name
                    }).ToList() ?? new System.Collections.Generic.List<ServerViewModel>(),
                    CurrentUser = currentUser
                });
            }
        }

        private ServerViewModel GetServerViewModel(DataContext ctx, DiscordUser currentUser, long discordServer)
        {
            var server = currentUser.Servers.First(x => x.Server.ServerId == discordServer).Server;
            var serverModel = new ServerViewModel
            {
                ServerId = discordServer,
                Modules = DiscordMetadata.Modules.Where(x => CheckModule(x.Name)).ToList(),
                ServerName = server.Name,
            };
            serverModel.Channels = server.Channels.Select(x => new DiscordChannelViewModel { ChannelId = x.ChannelId, ChannelName = x.Name, ParentServer = serverModel, DiscordChannelType = x.ChannelType }).ToList();
            serverModel.Roles = server.Roles.Where(x => x.Name != "@everyone").Select(x => new DiscordRoleViewModel { RoleId = x.RoleId, RoleName = x.Name, ParentServer = serverModel }).ToList();
            var botSettings = DiscordMetadata.BotSettings.OrderBy(x => x.Type).Select(x => new DiscordSettingViewModel
            {
                Key = x.Key,
                ParentServer = serverModel,
                Value = ctx.DiscordSettings.FirstOrDefault(s => s.Server.ServerId == discordServer && s.Key == x.Key)?.Value,
                SettingType = x.Type,
                SettingName = x.Name,
                Description = ParseDescription(x),
                ExtraArgs = x.ExtraArgs
            }).ToList();
            serverModel.BotSettings = botSettings;
            return serverModel;
        }

        [Route("manage/{discordServer}", Name = "DiscordManageServer")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult ManageServer(long discordServer)
        {
            using (var ctx = new DataContext())
            {
                var currentUser = ctx.DiscordUsers.First(x => x.UserId == _currentUserId);

                if (CheckMutualServer(discordServer, currentUser))
                    return View(GetServerViewModel(ctx, currentUser, discordServer));
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
                var currentUser = ctx.DiscordUsers.First(x => x.UserId == _currentUserId);

                if (CheckMutualServer(discordServer, currentUser))
                {
                    if (CheckModule(module))
                    {
                        var serverModel = GetServerViewModel(ctx, currentUser, discordServer);

                        var model = new ModuleViewModel
                        {
                            ParentServer = serverModel,
                            SelectedModule = DiscordMetadata.Modules.First(x => x.Name.ToLower() == module.ToLower())
                        };
                        var moduleSettings = model.SelectedModule.Settings.OrderBy(x => x.Type).Select(x => new DiscordSettingViewModel
                        {
                            Key = x.Key,
                            SettingName = x.Name,
                            Description = ParseDescription(x),
                            ParentServer = serverModel,
                            SettingType = x.Type,
                            Value = ctx.DiscordSettings.FirstOrDefault(s => s.Server.ServerId == discordServer && s.Key == x.Key)?.Value,
                            ExtraArgs = x.ExtraArgs,
                            Module = module
                        }).ToList();
                        model.ModuleSettings = moduleSettings;
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

        private string ParseDescription(Models.DiscordSetting setting)
        {
            var description = setting.Description;
            var matches = Regex.Matches(setting.Description, "[$]<([A-Z]*):([A-Z_]*)>");
            foreach(var match in matches.Cast<Match>().Where(x => x.Success))
            {
                var refSetting = GetRefSetting(match.Groups[1].Value, match.Groups[2].Value);
                description = description.Replace(match.Groups[0].Value, $"'{refSetting.Name}'");
            }

            return description;
        }

        private static Models.DiscordSetting GetRefSetting(string module, string key)
        {
            var settings = module == "GUILD"
                ? DiscordMetadata.BotSettings
                : DiscordMetadata.Modules.First(x => x.Name.ToUpper() == module).Settings;
            return settings.First(x => x.Key == key);
        }

        //[Route("manage/{discordServer}/{module}/{command}", Name = "DiscordManageCommand")]
        //[Authorize]
        //[MustHaveLoginProvider("discord", "DiscordHome")]
        //public ActionResult ManageCommand(long discordServer, string module, string command)
        //{
        //    using (var ctx = new DataContext())
        //    {
        //        var currentUser = ctx.DiscordUsers.First(x => x.UserId == _currentUserId);

        //        if (CheckMutualServer(discordServer, currentUser))
        //        {
        //            if (CheckCommand(module, command, currentUser.Servers.First(x => x.Server.ServerId == discordServer)))
        //            {
        //                var model = new CommandViewModel
        //                {
        //                    ServerId = discordServer,
        //                    SelectedModule = DiscordMetadata.Modules.First(x => x.Name.ToLower() == module)
        //                };
        //                model.SelectedCommand = model.SelectedModule.Commands.First(x => x.Name.ToLower() == command);
        //                return View(model);
        //            }
        //            else
        //            {
        //                TempData["Error"] = $"Could not find command '{command}', or you do not have access to it on this server";
        //                return RedirectToRoute("DiscordManageModule", new { discordServer, module });
        //            }
        //        }
        //        else
        //            return RedirectToRoute("DiscordManage");
        //    }
        //}

        [Route("save", Name = "DiscordSaveSetting")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        [HttpPost]
        public ActionResult SaveSetting(long discordServer, string module, string key, string value)
        {
            ActionResult ErrorResult() => new HttpUnauthorizedResult("User does not have required permission");
            using (var ctx = new DataContext())
            {
                var currentUser = ctx.DiscordUsers.FirstOrDefault(x => x.UserId == _currentUserId)?.Servers.FirstOrDefault(x => x.Server.ServerId == discordServer);
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
                        setting = ctx.DiscordSettings.FirstOrDefault(x => x.Server.ServerId == currentUser.Server.ServerId && x.Key == botSetting.Key);
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
                        setting = ctx.DiscordSettings.FirstOrDefault(x => x.Server.ServerId == currentUser.Server.ServerId && x.Key == moduleSetting.Key);
                        if (setting == null)
                            setting = ctx.DiscordSettings.Add(new Data.DataModels.DiscordModels.DiscordSetting { Server = currentUser.Server, Key = moduleSetting.Key });
                        setting.Value = value;
                    }
                }

                ctx.SaveChanges();
                return Json(JsonConvert.SerializeObject(new { result = 1, setting.Key, setting.Value }));
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
            var discordModule = DiscordMetadata.Modules.FirstOrDefault(x => x.Name.ToLower() == module.ToLower());
            if (discordModule == null)
                return false;

            if (discordModule.RequireOwner && _currentUserId != DiscordBotOwnerId)
                return false;

            return true;
        }

        //private bool CheckCommand(string module, string command, DiscordServerUser currentUser)
        //{
        //    if (CheckModule(module))
        //    {
        //        var discordCommand = DiscordMetadata.Modules.Where(x => x.Name.ToLower() == module).SelectMany(x => x.Commands).FirstOrDefault(x => x.Name.ToLower() == command);

        //        if (discordCommand == null)
        //            return false;

        //        if (discordCommand.RequireOwner && _currentUserId != DiscordBotOwnerId)
        //            return false;

        //        if (currentUser.Roles.Any(x => (x.DiscordPermissions & discordCommand.RequireGuildPermission) != 0))
        //            return true;
        //    }
        //    return false;
        //}
    }
}