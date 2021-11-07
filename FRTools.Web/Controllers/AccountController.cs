using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels;
using FRTools.Web.Infrastructure;
using FRTools.Web.Infrastructure.Managers;
using FRTools.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : BaseController
    {
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private SignInManager<User, int> _signInManager;
        private UserManager<User, int> _userManager;

        public SignInManager<User, int> SignInManager => _signInManager ?? (_signInManager = HttpContext.GetOwinContext().Get<SignInManager<User, int>>());

        public UserManager<User, int> UserManager => _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<User, int>>());

        [Route("login", Name = "Login")]
        public ActionResult Login(string returnUrl = null)
        {
            if (Request.IsAuthenticated)
                return returnUrl == null ? RedirectToRoute("Home") : RedirectToLocal(returnUrl);

            return View(new ExternalLoginListViewModel { ReturnUrl = returnUrl });
        }

        [Route("logout", Name = "LogOut")]
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AddInfoNotification("You have been logged out");
            return RedirectToRoute("Home");
        }

        [Route("delete", Name = "DeleteAccount")]
        [Authorize]
        public ActionResult DeleteAccount() => View(new DeleteAccountViewModel());

        [HttpPost]
        [Route("delete", Name = "DeleteAccountPost")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            if (model.Confirm != true)
                ModelState.AddModelError("Confirm", "You need to confirm just to be sure.");

            if (!ModelState.IsValid)
                return View(model);

            using (var ctx = new DataContext())
            {
                var user = ctx.Users
                    .Where(x => x.Id == LoggedInUser.Id)
                    .Include(x => x.Skins)
                    .Include(x => x.Previews)
                    .Include(x => x.Pinglists)
                    .First();

                var azureImageService = new AzureImageService();
                foreach (var skin in user.Skins)
                {
                    await azureImageService.DeleteImage($@"skins\{skin.GeneratedId}.png");
                    skin.Previews.Clear();
                }
                ctx.Skins.RemoveRange(user.Skins);
                foreach (var preview in user.Previews)
                {
                    preview.Requestor = null;
                }

                foreach (var pinglist in user.Pinglists)
                {
                    ctx.PingListEntries.RemoveRange(pinglist.Entries);
                }
                ctx.Pinglists.RemoveRange(user.Pinglists);

                var pinglistCategories = ctx.PinglistCategories.Where(x => x.Owner.Id == user.Id).ToList();
                ctx.PinglistCategories.RemoveRange(pinglistCategories);

                if(user.FRUser != null)
                    user.FRUser.User = null;
                await ctx.SaveChangesAsync();
            }

            foreach (var login in LoggedInUser.Logins)
            {
                if(login.LoginProvider == "discord")
                {
                    using (var ctx = new DataContext())
                    {
                        ctx.DiscordSettings.RemoveRange(ctx.DiscordSettings.Where(x => x.Key.EndsWith("USER_" + login.ProviderKey)).ToList());
                        await ctx.SaveChangesAsync();
                    }
                }
                await UserManager.RemoveLoginAsync(LoggedInUser.Id, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }

            using (var ctx = new DataContext())
            {
                var user = ctx.Users.Find(LoggedInUser.Id);

                ctx.Users.Remove(user);
                await ctx.SaveChangesAsync();
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AddInfoNotification("Your account has been deleted and you have been logged out");

            return RedirectToRoute("Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("externalLogin")]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.RouteUrl("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        private static long GetDiscordUserId(ExternalLoginInfo loginInfo)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {loginInfo.ExternalIdentity.FindFirstValue("urn:discord:accesstoken")}");
                return JsonConvert.DeserializeObject<dynamic>(client.DownloadString("https://discordapp.com/api/users/@me")).id;
            }
        }

        [AllowAnonymous]
        [Route("externalLoginCallback", Name = "ExternalLoginCallback")]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl, string error)
        {
            if (error == "access_denied")
            {
                AddErrorNotification("Login failed.");
                return RedirectToRoute("Login");
            }

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                AddErrorNotification("Could not retrieve external login information from request, please try again.<br/>If the issue persists then please let me know <a href=\"https://github.com/Nensec/FRTools/issues/5\">here</a>.");
                return RedirectToRoute("Login");
            }
            loginInfo.Login.ProviderKey = GetProviderData(loginInfo);

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        var user = await UserManager.FindAsync(loginInfo.Login);
                        var providerData = JsonConvert.DeserializeObject<UserStore.ProviderData>(loginInfo.Login.ProviderKey);
                        var providerLogin = user.Logins.First(x => x.ProviderKey == providerData.ProviderKey && x.LoginProvider == loginInfo.Login.LoginProvider);
                        if (providerLogin.ProviderUsername == null)
                        {
                            providerLogin.ProviderUsername = providerData.ProviderUsername;
                            await UserManager.UpdateAsync(user);
                        }

                        if (providerData.AccessToken != null)
                        {
                            SetDiscordAccessToken(user.Id, providerData.AccessToken);
                        }

                        if (returnUrl != null)
                            return RedirectToLocal(returnUrl);
                        return RedirectToRoute("Home");
                    }
                case SignInStatus.Failure:
                default:
                    if (ModelState.IsValid)
                    {
                        async Task<(IdentityResult, User)> CreateNewUser(string usernameAffix = null)
                        {
                            var providerData = JsonConvert.DeserializeObject<UserStore.ProviderData>(loginInfo.Login.ProviderKey);
                            var user = new User
                            {
                                UserName = providerData.ProviderUsername + usernameAffix,
                                Email = loginInfo.Email,
                                ProfileSettings = new ProfileSettings()
                            };

                            return (await UserManager.CreateAsync(user), user);
                        };

                        async Task<ActionResult> LoginUser(User user)
                        {
                            var loginResult = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                            if (loginResult.Succeeded)
                            {
                                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                                var providerData = JsonConvert.DeserializeObject<UserStore.ProviderData>(loginInfo.Login.ProviderKey);
                                if (providerData.AccessToken != null)
                                    SetDiscordAccessToken(user.Id, providerData.AccessToken);
                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                AddErrorNotification($"Could not login user:<br/><br/><ul>{string.Join("", loginResult.Errors.Select(x => $"<li>{x}</li>"))}</ul>");
                                return RedirectToRoute("Login");
                            }
                        };

                        string externalUsername = loginInfo.DefaultUserName ?? loginInfo.ExternalIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                        var (newUser, identity) = await CreateNewUser();

                        if (newUser.Succeeded)
                            return await LoginUser(identity);
                        else if (!identity.UserName.All(x => char.IsDigit(x)))
                        {
                            (newUser, identity) = await CreateNewUser("_" + CodeHelpers.GenerateId(5));
                            if (newUser.Succeeded)
                                return await LoginUser(identity);
                            AddErrorNotification($"Could not create user:<br/><br/><ul>{string.Join("", newUser.Errors.Select(x => $"<li>{x}</li>"))}</ul>");
                            return RedirectToRoute("Login");
                        }
                    }

                    AddErrorNotification("Something went wrong submitting the request");

                    ViewBag.ReturnUrl = returnUrl;
                    return RedirectToRoute("Login");
            }
        }

        [Route("removeLogin", Name = "ManageRemoveLogin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId<int>(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                AddSuccessNotification($"Succesfully removed the specified {loginProvider} login from your account!");
            }
            else
                AddErrorNotification(result.Errors.First());
            return RedirectToRoute("ManageLogins");
        }

        [Route("externalLinkLogin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            return new ChallengeResult(provider, Url.RouteUrl("ExternalLoginLinkCallback"), User.Identity.GetUserId());
        }

        [Route("externalLoginLinkCallback", Name = "ExternalLoginLinkCallback")]
        public async Task<ActionResult> LinkLoginCallback(string error)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(ChallengeResult._xsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                AddErrorNotification("Could not retrieve external login information from request, please try again.<br/>If the issue persists then please let me know <a href=\"https://github.com/Nensec/FRTools/issues/5\">here</a>.");
                return RedirectToRoute("ManageLogins");
            }

            loginInfo.Login.ProviderKey = GetProviderData(loginInfo);

            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId<int>(), loginInfo.Login);
            if (!result.Succeeded)
                AddErrorNotification(result.Errors.First());
            else
            {
                var providerData = JsonConvert.DeserializeObject<UserStore.ProviderData>(loginInfo.Login.ProviderKey);
                if (providerData.AccessToken != null)                
                    SetDiscordAccessToken(User.Identity.GetUserId<int>(), providerData.AccessToken);
                
                AddSuccessNotification($"Succesfully added your {loginInfo.Login.LoginProvider} login to your account!");
            }

            return RedirectToRoute("ManageLogins");
        }

        private void SetDiscordAccessToken(int userId, string accessToken)
        {
            foreach (var claim in UserManager.GetClaims(userId))
                UserManager.RemoveClaim(userId, claim);

            UserManager.AddClaim(userId, new Claim("DiscordAccessToken", accessToken));
        }

        private string GetProviderData(ExternalLoginInfo loginInfo)
        {
            var providerData = new UserStore.ProviderData();
            switch (loginInfo.Login.LoginProvider.ToLower())
            {
                case "discord":
                    providerData.ProviderKey = GetDiscordUserId(loginInfo).ToString();
                    providerData.ProviderUsername = $"{loginInfo.ExternalIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")}#{loginInfo.ExternalIdentity.FindFirstValue("urn:discord:discriminator")}";
                    providerData.AccessToken = loginInfo.ExternalIdentity.FindFirstValue("urn:discord:accesstoken");
                    break;
                default:
                    providerData.ProviderKey = loginInfo.Login.ProviderKey;
                    providerData.ProviderUsername = loginInfo.DefaultUserName ?? loginInfo.ExternalIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    break;
            }
            return JsonConvert.SerializeObject(providerData);
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