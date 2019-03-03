using FRSkinTester.Infrastructure;
using FRSkinTester.Models;
using Microsoft.AspNet.Identity;
using System;
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
        [Route("Account", Name = "ManageAccount")]
        public ActionResult Index() => View();

        // Opting to just store the guids in memory rather than save them in the database
        static MemoryCache _verifyCache = new MemoryCache("verifyCache");


        /* TODO:
         * make user account editable
         * allow linking of existing secret keys to account
         * make new uploads automagically tied to account if logged it
         * make previews tied to account if logged in
        */

        public ActionResult EditUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditUser(object model)
        {
            return View();
        }

        public ActionResult LinkExistingSkin(string skinId, string secretKey)
        {
            return View();
        }

        [Route("Account/Verify", Name = "VerifyFR")]
        public ActionResult Verify()
        {
            var userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
            Guid? verifyGuid = Guid.NewGuid();
            verifyGuid = (Guid?)_verifyCache.AddOrGetExisting($"USER_{userId}", verifyGuid, DateTimeOffset.Now.AddMinutes(5)) ?? verifyGuid;

            return View(new VerifyFRViewModel { Guid = verifyGuid.Value });
        }

        [Route("Account/Verify", Name = "VerifyFRPost")]
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