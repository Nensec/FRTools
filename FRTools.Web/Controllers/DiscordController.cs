using FRTools.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("discord")]
    public class DiscordController : BaseController
    {
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
        public ActionResult Manage(long? discordServer)
        {
            return View("Index");
        }

        [Route("save", Name = "DiscordSaveSetting")]
        [MustHaveLoginProvider("discord", "DiscordHome")]
        public ActionResult SaveSetting(long discordServer, string key, string value)
        {
            return Json("");
        }
    }
}