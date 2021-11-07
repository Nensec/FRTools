using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels;
using FRTools.Data.DataModels.DiscordModels.RestModels;
using FRTools.Data.Messages;
using FRTools.Web.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("discord")]
    public class DiscordController : BaseController
    {
        private static DiscordMetadata DiscordMetadata { get; }
        private static long DiscordBotOwnerId { get; }
        private RestClient UserDiscordClient { get; }
        private RestClient BotDiscordClient { get; }

        private MemoryCache Cache { get; } = MemoryCache.Default;

        static DiscordController()
        {
            var json = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/bin/DiscordMetadata.json"));
            DiscordMetadata = JsonConvert.DeserializeObject<DiscordMetadata>(json);
            if (long.TryParse(ConfigurationManager.AppSettings["DiscordBotOwner"], out var id))
                DiscordBotOwnerId = id;
        }

        public DiscordController()
        {
            ViewBag.PngLogo = "/Content/frtools_discord.png";
            ViewBag.Logo = "/Content/frtools_discord.svg";
            UserDiscordClient = new RestClient("https://discord.com/api");
            UserDiscordClient.UseNewtonsoftJson();
            BotDiscordClient = new RestClient("https://discord.com/api");
            BotDiscordClient.UseNewtonsoftJson();
            BotDiscordClient.Authenticator = new BotAuthenticator(ConfigurationManager.AppSettings["DiscordToken"]);
        }

        private long? _currentUserId;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.IsAuthenticated)
            {
                _currentUserId = Session["CurrentDiscordUser"] as long?;
                var owin = HttpContext.GetOwinContext();
                var userManager = owin.GetUserManager<UserManager<User, int>>();
                var userId = HttpContext.User.Identity.GetUserId<int>();
                if (_currentUserId == null)
                {
                    var logins = userManager.GetLogins(userId);
                    var discordLogin = logins.FirstOrDefault(x => x.LoginProvider.ToLower() == "discord");
                    if (discordLogin != null)
                        Session["CurrentDiscordUser"] = _currentUserId = long.Parse(discordLogin.ProviderKey);
                }
                var claims = userManager.GetClaims(userId);
                var accessTokenClaim = claims.FirstOrDefault(x => x.Type == "DiscordAccessToken");
                if (accessTokenClaim != null)
                    UserDiscordClient.Authenticator = new JwtAuthenticator(accessTokenClaim.Value);
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
            var model = new DiscordHelpViewModel();
            model.Modules = DiscordMetadata.Modules.Where(x => !x.RequireOwner).Select(x => new DiscordModuleHelpViewModel
            {
                Module = x,
                Commands = x.Commands.Select(c => new DiscordCommandHelpViewModel { Command = c }).ToList()
            }).ToList();
            return View(model);
        }

        [Route("help/{module}/{command}")]
        public ActionResult CommandHelp(string module, string command)
        {
            var discordModule = DiscordMetadata.Modules.FirstOrDefault(x => x.Name.ToLower() == module.ToLower());
            if (discordModule != null)
            {
                var moduleCommand = discordModule.Commands.FirstOrDefault(x => x.Name.ToLower() == command.ToLower());
                if (moduleCommand != null)
                {
                    return PartialView("_CommandHelp", moduleCommand);
                }
            }
            return HttpNotFound();
        }

        [Route("manage", Name = "DiscordManage")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public async Task<ActionResult> Manage()
        {
            var userServers = await GetUserGuilds();

            if (userServers == null)
                return RedirectToRoute("Home");

            var userServerIds = userServers.Select(x => x.Id).ToArray();
            var matchingServerIds = DataContext.DiscordServers.Where(x => userServerIds.Contains(x.ServerId)).Select(x => x.ServerId);

            var matchingServers = userServers.Where(x => matchingServerIds.Contains(x.Id) && (x.Owner || (x.Permissions & 8) != 0)).ToList();

            if (!matchingServers.Any())
                TempData["Warning"] = "You don't appear to share a server with a bot";

            var vm = new ServersViewModel();
            foreach (var server in matchingServers)
            {
                vm.Servers.Add(new ServerViewModel
                {
                    ServerId = server.Id,
                    IconHash = server.Icon,
                    ServerName = server.Name
                });
            }

            return View(vm);
        }

        private async Task<ServerViewModel> GetServerViewModel(long serverId)
        {
            var serverModel = (ServerViewModel)Cache[$"ServerModel_{serverId}"];

            if (serverModel == null)
            {

                var guildResponse = await BotDiscordClient.ExecuteGetAsync<DiscordGuild>(new RestRequest($"/guilds/{serverId}"));
                var guildChannelsResponse = await BotDiscordClient.ExecuteGetAsync<List<DiscordChannel>>(new RestRequest($"/guilds/{serverId}/channels"));

                if (!guildResponse.IsSuccessful || !guildChannelsResponse.IsSuccessful)
                {
                    AddErrorNotification("Something went wrong fetching data from Discord, try again later or contact me<br/><br/>" + (guildResponse.IsSuccessful ? guildResponse.ErrorMessage : guildChannelsResponse.ErrorMessage));
                    return null;
                }

                serverModel = new ServerViewModel
                {
                    ServerId = serverId,
                    Modules = DiscordMetadata.Modules.Where(x => CheckModule(x.Name)).ToList(),
                    ServerName = guildResponse.Data.Name,
                };
                serverModel.Channels = guildChannelsResponse.Data.Select(x => new DiscordChannelViewModel { ChannelId = x.Id, ChannelName = x.Name, ParentServer = serverModel, DiscordChannelType = (DiscordChannelType)x.Type }).ToList();
                serverModel.Roles = guildResponse.Data.Roles.Where(x => x.Name != "@everyone").Select(x => new DiscordRoleViewModel { RoleId = x.Id, RoleName = x.Name, ParentServer = serverModel }).ToList();
                var botSettingCategories = DiscordMetadata.BotSettingCategories.OrderBy(x => x.Name).ToList();
                var botSettings = DiscordMetadata.BotSettings.OrderBy(x => x.Order).ThenBy(x => x.Type).Select(x => new DiscordSettingViewModel
                {
                    Key = x.Key,
                    ParentServer = serverModel,
                    Value = DataContext.DiscordSettings.FirstOrDefault(s => s.Server.ServerId == serverId && s.Key == x.Key)?.Value ?? x.DefaultValue,
                    SettingType = x.Type,
                    SettingName = x.Name,
                    Description = ParseDescription(x),
                    ExtraArgs = x.ExtraArgs,
                    Category = x.Category
                }).ToList();
                serverModel.BotSettingCategories = botSettingCategories;
                serverModel.BotSettings = botSettings;

                Cache.Add($"ServerModel_{serverId}", serverModel, DateTimeOffset.UtcNow.AddMinutes(5));
            }

            return serverModel;
        }

        [Route("manage/{discordServer}", Name = "DiscordManageServer")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public async Task<ActionResult> ManageServer(long discordServer)
        {
            if (await CheckMutualServer(discordServer))
                return View(await GetServerViewModel(discordServer));
            else
                return RedirectToRoute("DiscordManage");
        }

        [Route("manage/{discordServer}/{module}", Name = "DiscordManageModule")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public async Task<ActionResult> ManageModule(long discordServer, string module)
        {
            if (await CheckMutualServer(discordServer))
            {
                if (CheckModule(module))
                {
                    var serverModel = await GetServerViewModel(discordServer);

                    var model = new DiscordModuleViewModel
                    {
                        ParentServer = serverModel,
                        SelectedModule = DiscordMetadata.Modules.First(x => x.Name.ToLower() == module.ToLower())
                    };
                    var moduleSettings = model.SelectedModule.Settings.OrderBy(x => x.Order).ThenBy(x => x.Type).Select(x => new DiscordSettingViewModel
                    {
                        Key = x.Key,
                        SettingName = x.Name,
                        Description = ParseDescription(x),
                        ParentServer = serverModel,
                        SettingType = x.Type,
                        Value = DataContext.DiscordSettings.FirstOrDefault(s => s.Server.ServerId == discordServer && s.Key == x.Key)?.Value ?? x.DefaultValue,
                        ExtraArgs = x.ExtraArgs,
                        Module = module
                    }).ToList();
                    model.Settings = moduleSettings;
                    model.Categories = model.SelectedModule.SettingCategories;
                    model.Commands = model.SelectedModule.Commands.Select(x => new DiscordCommandViewModel
                    {
                        ParentServer = serverModel,
                        SelectedModule = model.SelectedModule,
                        SelectedCommand = x,
                        Settings = x.Settings.OrderBy(s => s.Order).ThenBy(s => s.Type).Select(s => new DiscordSettingViewModel
                        {
                            Key = s.Key,
                            SettingName = s.Name,
                            Description = ParseDescription(s),
                            ParentServer = serverModel,
                            SettingType = s.Type,
                            Value = DataContext.DiscordSettings.FirstOrDefault(cs => cs.Server.ServerId == discordServer && cs.Key == s.Key)?.Value ?? s.DefaultValue,
                            ExtraArgs = s.ExtraArgs,
                            Module = module,
                            Command = x.Name
                        }).ToList()
                    }).ToList();
                    return View(model);
                }
                else
                {
                    AddErrorNotification($"Could not find module '{module}', or you do not have access to it on this server");
                    return RedirectToRoute("DiscordManageServer", new { discordServer });
                }
            }
            else
                return RedirectToRoute("DiscordManage");

        }

        private string ParseDescription(Common.DiscordSetting setting)
        {
            var description = setting.Description;
            if (description != null)
            {
                var matches = Regex.Matches(setting.Description, "[$]<([A-Z]*):([A-Z_]*)>");
                foreach (var match in matches.Cast<Match>().Where(x => x.Success))
                {
                    var refSetting = GetRefSetting(match.Groups[1].Value, match.Groups[2].Value);
                    description = description.Replace(match.Groups[0].Value, $"<span class=\"text-warning\"><b>{refSetting.Name}</b> ({(match.Groups[1].Value == "GUILD" ? "Server" : match.Groups[2].Value)} settings)</span>");
                }
            }
            return description;
        }

        private static Common.DiscordSetting GetRefSetting(string module, string key)
        {
            var settings = module == "GUILD"
                ? DiscordMetadata.BotSettings
                : DiscordMetadata.Modules.First(x => x.Name.ToUpper() == module).Settings;
            return settings.First(x => x.Key == key);
        }

        [Route("save", Name = "DiscordSaveSetting")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        [HttpPost]
        public async Task<ActionResult> SaveSetting(string discordServer, string module, string key, string value)
        {
            var discordServerId = long.Parse(discordServer);
            ActionResult ErrorResult() => new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "User does not have required permission");

            Data.DataModels.DiscordModels.DiscordSetting setting = null;
            if (module == null)
            {
                var botSetting = DiscordMetadata.BotSettings.FirstOrDefault(x => x.Key.ToLower() == key.ToLower());
                if (botSetting == null)
                    return ErrorResult();

                if (await CheckMutualServer(discordServerId))
                {
                    setting = DataContext.DiscordSettings.FirstOrDefault(x => x.Server.ServerId == discordServerId && x.Key == botSetting.Key);
                    if (setting == null)
                        setting = DataContext.DiscordSettings.Add(new Data.DataModels.DiscordModels.DiscordSetting { Server = DataContext.DiscordServers.First(x => x.ServerId == discordServerId), Key = botSetting.Key });
                    setting.Value = value;
                }
            }
            else
            {
                var discordModule = DiscordMetadata.Modules.FirstOrDefault(x => x.Name.ToLower() == module.ToLower());
                if (discordModule == null)
                    return ErrorResult();

                var moduleSetting = discordModule.Settings.Concat(discordModule.Commands.SelectMany(x => x.Settings)).FirstOrDefault(x => x.Key.ToLower() == key.ToLower());
                if (moduleSetting == null)
                    return ErrorResult();

                if (await CheckMutualServer(discordServerId))
                {
                    setting = DataContext.DiscordSettings.FirstOrDefault(x => x.Server.ServerId == discordServerId && x.Key == moduleSetting.Key);
                    if (setting == null)
                        setting = DataContext.DiscordSettings.Add(new Data.DataModels.DiscordModels.DiscordSetting { Server = DataContext.DiscordServers.First(x => x.ServerId == discordServerId), Key = moduleSetting.Key });
                    setting.Value = value;
                }
            }

            DataContext.SaveChanges();
            var _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
            await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.SettingUpdated, setting.Key) { DiscordServer = discordServerId }))));

            return Json(JsonConvert.SerializeObject(new { result = 1, setting.Key, setting.Value }));
        }

        [Route("test", Name = "DiscordSendTestMessage")]
        [Authorize]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        [HttpPost]
        public async Task<ActionResult> SendTestMessage(string discordServer, string module, string key)
        {
            var discordServerId = long.Parse(discordServer);
            ActionResult ErrorResult() => new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "User does not have required permission");

            bool IsChannelType(string settingType)
            {
                var type = Type.GetType(settingType);
                var fallBack = settingType.Split(',')[0];
                if (fallBack.Contains("."))
                {
                    fallBack = fallBack.Split('.')[1];
                }
                return (type?.Name ?? fallBack) == "ITextChannel";
            }

            Data.DataModels.DiscordModels.DiscordSetting setting = null;
            if (module == null)
            {
                var botSetting = DiscordMetadata.BotSettings.FirstOrDefault(x => x.Key.ToLower() == key.ToLower());
                if (botSetting == null)
                    return ErrorResult();

                if (IsChannelType(botSetting.Type) && await CheckMutualServer(discordServerId))
                    setting = DataContext.DiscordSettings.FirstOrDefault(x => x.Server.ServerId == discordServerId && x.Key == botSetting.Key);
            }
            else
            {
                var discordModule = DiscordMetadata.Modules.FirstOrDefault(x => x.Name.ToLower() == module.ToLower());
                if (discordModule == null)
                    return ErrorResult();

                var moduleSetting = discordModule.Settings.Concat(discordModule.Commands.SelectMany(x => x.Settings)).FirstOrDefault(x => x.Key.ToLower() == key.ToLower());
                if (moduleSetting == null)
                    return ErrorResult();

                if (IsChannelType(moduleSetting.Type) && await CheckMutualServer(discordServerId))
                    setting = DataContext.DiscordSettings.FirstOrDefault(x => x.Server.ServerId == discordServerId && x.Key == moduleSetting.Key);
            }

            var _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
            await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.DiscordTestMessage, setting.Key) { DiscordServer = discordServerId }))));

            return Json(JsonConvert.SerializeObject(new { result = 1, setting.Key, setting.Value }));

        }

        private async Task<List<DiscordGuild>> GetUserGuilds()
        {
            var userServers = (List<DiscordGuild>)Cache[$"UserServers_{User.Identity.GetUserId<int>()}"];

            if (userServers == null)
            {
                var userServersResponse = await UserDiscordClient.ExecuteGetAsync<List<DiscordGuild>>(new RestRequest("/users/@me/guilds"));

                if (!userServersResponse.IsSuccessful)
                {
                    if (userServersResponse.Content.Contains("Unauthorized") && userServersResponse.Content.Contains("401"))                    
                        AddErrorNotification("The website was not authorized to fetch data from Discord, your access token might have expired. Try logging out and back in with your Discord account then try again.");                    
                    else                    
                        AddErrorNotification("Something went wrong fetching data from Discord, try again later or contact me.<br/>Alternatively try logging out and back in using your Discord account, your access token might have expired.<br/><br/>" + (userServersResponse.ErrorMessage ?? userServersResponse.Content));
                    
                    return null;
                }

                Cache.Add($"UserServers_{User.Identity.GetUserId<int>()}", userServers = userServersResponse.Data, DateTimeOffset.UtcNow.AddMinutes(5));
            }
            return userServers;
        }

        private async Task<bool> CheckMutualServer(long discordServer)
        {
            var userServers = await GetUserGuilds();
            if (userServers == null)
                return false;

            var userServer = userServers.FirstOrDefault(x => x.Id == discordServer);

            if (userServer == null)
            {
                AddErrorNotification($"User is not in server {discordServer}");
                return false;
            }
            if (DataContext.DiscordServers.Any(x => x.ServerId == userServer.Id) && (userServer.Owner || (userServer.Permissions & 8) != 0))
                return true;
            else
            {
                AddErrorNotification($"Could not find server id '{discordServer}' in the list of mutual servers where you are an administrator between you and the bot");
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
    }
}