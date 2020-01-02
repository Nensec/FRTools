using FRTools.Data.DataModels;
using FRTools.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FRTools.Data;
using FRTools.Web.Infrastructure;
using Microsoft.Owin.Security;

namespace FRTools.Web.Controllers
{

    [Authorize]
    [RoutePrefix("manage")]
    public class ManageController : BaseController
    {
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private SignInManager<User, int> _signInManager;
        private UserManager<User, int> _userManager;

        public SignInManager<User, int> SignInManager => _signInManager ?? (_signInManager = HttpContext.GetOwinContext().Get<SignInManager<User, int>>());

        public UserManager<User, int> UserManager => _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<User, int>>());


        [Route(Name = "ManageAccount")]
        public ActionResult Index()
        {
            var model = new AccountViewModel();
            var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
            using (var ctx = new DataContext())
            {
                var user = ctx.Users
                    .Include(x => x.Skins.Select(s => s.Previews))
                    .Include(x => x.FRUser)
                    .FirstOrDefault(x => x.Id == userid);
                model.User = user;
                model.Skins = user.Skins.ToList();
            }
            return View(model);
        }

        // Opting to just store the guids in memory rather than save them in the database
        static MemoryCache _verifyCache = new MemoryCache("verifyCache");

        [Route("account", Name = "ManageUser")]
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
                    Privacy = user.Privacy,
                    DefaultVisibility = user.DefaultVisibility
                };
                return View(model);
            }
        }

        [HttpPost]
        [Route("account", Name = "ManageUserPost")]
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
                    user.DefaultVisibility = model.DefaultVisibility;
                    await ctx.SaveChangesAsync();
                    await HttpContext.GetOwinContext().Get<SignInManager<User, int>>().SignInAsync(user, true, true);
                }
                TempData["Success"] = "Changes have been saved!";
                return RedirectToRoute("ManageAccount");
            }
            catch (Exception ex)
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

        [Route("verify", Name = "VerifyFR")]
        public ActionResult Verify()
        {
            var userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
            Guid? verifyGuid = Guid.NewGuid();
            verifyGuid = (Guid?)_verifyCache.AddOrGetExisting($"USER_{userId}", verifyGuid, DateTimeOffset.Now.AddMinutes(5)) ?? verifyGuid;

            return View(new VerifyFRViewModel { Guid = verifyGuid.Value });
        }

        [Route("verify", Name = "VerifyFRPost")]
        [HttpPost]
        public async Task<ActionResult> Verify(VerifyFRPostViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var frUser = await FRHelpers.GetOrUpdateFRUser(model.LairId, ctx);

                if (frUser.User != null)
                {
                    TempData["Error"] = "This lair id is already tied to an account, if you believe this is an error please contact nensec#1337 on Discord";
                    RedirectToRoute("VerifyFR");
                }

                var userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();

                var key = $"USER_{userId}";
                if (_verifyCache.Contains(key))
                {
                    var verifyGuid = (Guid)_verifyCache.Get(key);
                    using (var client = new WebClient())
                    {
                        var profilePage = await client.DownloadStringTaskAsync($@"https://www1.flightrising.com/clan-profile/{model.LairId}");
                        var bioTagIndex = profilePage.IndexOf("<div id=\"userbio\">");
                        if (profilePage.Substring(bioTagIndex).Contains(verifyGuid.ToString()))
                        {
                            _verifyCache.Remove(key);
                            {
                                var dbUser = ctx.Users.Find(userId);
                                dbUser.FRUser = frUser;

                                await ctx.SaveChangesAsync();
                            }

                            TempData["Success"] = $"Succesfully linked your Flight Rising account ({frUser.Username})!";
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

        [Route("logins", Name = "ManageLogins")]
        [Authorize]
        public async Task<ActionResult> ManageLogins()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = user.Logins.ToList(),
                ShowRemoveButton = user.Logins.Count > 1
            });
        }
    }
}