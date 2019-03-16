using FRSkinTester.Infrastructure;
using FRSkinTester.Infrastructure.DataModels;
using FRSkinTester.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FRSkinTester.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        [Route("Manage", Name = "ManageAccount")]
        public ActionResult Index()
        {
            var model = new AccountViewModel
            {
                CDNBasePath = ConfigurationManager.AppSettings["CDNBasePath"]
            };
            var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
            using (var ctx = new DataContext())
            {
                var user = ctx.Users
                    .Include(x => x.Skins.Select(s => s.Previews))
                    .FirstOrDefault(x => x.Id == userid);
                model.User = user;
                model.Skins = user.Skins.ToList();
            }
            return View(model);
        }

        // Opting to just store the guids in memory rather than save them in the database
        static MemoryCache _verifyCache = new MemoryCache("verifyCache");

        [Route("Manage/Account", Name = "ManageUser")]
        public ActionResult ManageUser()
        {
            var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
            using (var ctx = new DataContext())
            {
                var user = ctx.Users.Find(userid);

                var model = new UserPostViewModel
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Privacy = user.Privacy
                };
                return View(model);
            }
        }

        [HttpPost]
        [Route("Manage/Account", Name = "ManageUserPost")]
        public async Task<ActionResult> ManageUser(UserPostViewModel model)
        {
            try
            {
                var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
                using (var ctx = new DataContext())
                {
                    var user = ctx.Users.Find(userid);
                    user.UserName = string.IsNullOrWhiteSpace(model.Username) ? user.UserName : model.Username;
                    user.Email = model.Email;
                    user.Privacy = model.Privacy;
                    await ctx.SaveChangesAsync();
                    await HttpContext.GetOwinContext().Get<SignInManager<User, int>>().SignInAsync(user, true, true);
                }
                TempData["Success"] = "Changes have been saved!";
                return RedirectToRoute("ManageAccount");
            }
            catch(Exception ex)
            {
                var actualException = ex;
                while (actualException.InnerException != null)
                    actualException = actualException.InnerException;

                if (actualException is SqlException sqlEx && sqlEx.Number == 2601)
                    TempData["Error"] = "That username is already taken, please pick a different one";
                else
                    TempData["Error"] = "Something went wrong with your request";
                return View();
            }
        }

        [Route("Manage/Link", Name = "LinkExisting")]
        public ActionResult LinkExistingSkin() => View();

        [HttpPost]
        [Route("Manage/Link", Name = "LinkExistingPost")]
        public ActionResult LinkExistingSkin(ClaimSkinPostViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found or secret invalid";
                    return View();
                }
                int userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();

                if (skin.Creator != null)
                {
                    TempData["Error"] = "This skin is already linked to an acocunt, skins can only be claimed by a single account.";
                    return View();
                }

                skin.Creator = ctx.Users.Find(userId);
                ctx.SaveChanges();
            }
            TempData["Success"] = $"Succesfully linked skin '{model.SkinId}' to your account!";
            return RedirectToRoute("ManageAccount");
        }

        [Route("Manage/Verify", Name = "VerifyFR")]
        public ActionResult Verify()
        {
            var userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
            Guid? verifyGuid = Guid.NewGuid();
            verifyGuid = (Guid?)_verifyCache.AddOrGetExisting($"USER_{userId}", verifyGuid, DateTimeOffset.Now.AddMinutes(5)) ?? verifyGuid;

            return View(new VerifyFRViewModel { Guid = verifyGuid.Value });
        }

        [Route("Manage/Verify", Name = "VerifyFRPost")]
        [HttpPost]
        public async Task<ActionResult> Verify(VerifyFRPostViewModel model)
        {
            using (var ctx = new DataContext())
            {
                if (ctx.Users.FirstOrDefault(x => x.FRId == model.LairId) != null)
                {
                    TempData["Error"] = "This lair id is already tied to an account, if you believe this is an error please contact Nensec#435995 on FR";
                    RedirectToRoute("VerifyFR");
                }
            }

            var userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();

            var key = $"USER_{userId}";
            if (_verifyCache.Contains(key))
            {
                var verifyGuid = (Guid)_verifyCache.Get(key);
                using (var client = new WebClient())
                {
                    var profilePage = await client.DownloadStringTaskAsync($@"http://www1.flightrising.com/clan-profile/{model.LairId}");
                    var bioTagIndex = profilePage.IndexOf("<div id=\"userbio\">");
                    if (profilePage.Substring(bioTagIndex).Contains(verifyGuid.ToString()))
                    {
                        _verifyCache.Remove(key);
                        var userDataIndex = profilePage.IndexOf("<div id=\"userdata\"");
                        var userName = Regex.Match(profilePage.Substring(userDataIndex), @"<span+\s+[a-zA-Z]+\s*=\s*(""[^""]*""|'[^']*'*)\s*>[\r\n\t]*([a-zA-Z]*)[\t]*</span>").Groups[2].Value;
                        using (var ctx = new DataContext())
                        {
                            var dbUser = ctx.Users.Find(userId);
                            dbUser.FRId = model.LairId;
                            dbUser.FRName = userName;

                            await ctx.SaveChangesAsync();
                        }

                        TempData["Success"] = $"Succesfully linked your Flight Rising account ({userName})!";
                        return RedirectToRoute("ManageAccount");
                    }
                    else
                    {
                        TempData["Error"] = "Verify token was not found in your bio, please double check you saved!";
                        return RedirectToRoute("VerifyFR");
                    }
                }
            }
            else
            {
                TempData["Error"] = "Verify token expired, please try again!";
                return RedirectToRoute("VerifyFR");
            }
        }
    }
}