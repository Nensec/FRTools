using FRTools.Web.Infrastructure;
using FRTools.Web.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("discord")]
    public class DiscordController : BaseController
    {
        private static readonly Dictionary<string, DiscordModule> _modules = new Dictionary<string, DiscordModule>();

        static DiscordController()
        {
            var json = System.IO.File.ReadAllText("DiscordMetadata.json");
            var modules = JsonConvert.DeserializeObject<DiscordModule[]>(json);
            foreach (var module in modules)
                _modules.Add(module.Aliases[0], module);
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
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult Manage()
        {
            return View();
        }

        [Route("manage/{discordServer}", Name = "DiscordManageServer")]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult Manage(long discordServer)
        {
            return View();
        }

        [Route("manage/{discordServer}/{module}", Name = "DiscordManageModule")]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult ManageModule(long discordServer, string module)
        {
            return View();
        }

        [Route("manage/{discordServer}/{module}/{command}", Name = "DiscordManageCommand")]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult ManageCommand(long discordServer, string module, string command)
        {
            return View();
        }

        [Route("save", Name = "DiscordSaveSetting")]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult SaveSetting(long? discordServer, string key, string value)
        {
            return Json("");
        }
    }
}