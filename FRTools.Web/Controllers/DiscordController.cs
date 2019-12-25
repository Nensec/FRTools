using FRTools.Data.DataModels;
using FRTools.Data.DataModels.DiscordModels;
using FRTools.Web.Infrastructure;
using FRTools.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("discord")]
    public class DiscordController : BaseController
    {
        static DiscordMetadata DiscordMetadata { get; }

        static DiscordController()
        {
            var json = System.IO.File.ReadAllText("DiscordMetadata.json");
            DiscordMetadata = JsonConvert.DeserializeObject<DiscordMetadata>(json);
        }

        DiscordUser CurrentUser { get; }

        public DiscordController()
        {
            if (Request.IsAuthenticated)
            {
                var owin = HttpContext.GetOwinContext();
                var userManager = owin.GetUserManager<UserManager<User, int>>();
                var logins = userManager.GetLogins(HttpContext.User.Identity.GetUserId<int>());
                var discordLogin = logins.FirstOrDefault(x => x.LoginProvider.ToLower() == "discord");
                if(discordLogin != null)
                {
                    var id = discordLogin.ProviderKey;
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