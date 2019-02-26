using FRSkinTester.Infrastructure;
using FRSkinTester.Infrastructure.DataModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FRSkinTester.Controllers
{
    public class AccountController : BaseController
    {
        private SignInManager<User, int> _signInManager;
        private UserManager<User, int> _userManager;

        public SignInManager<User, int> SignInManager => _signInManager ?? (_signInManager = HttpContext.GetOwinContext().Get<SignInManager<User, int>>());

        public UserManager<User, int> UserManager => _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<User, int>>());

        [Route("Login", Name = "Login")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("LogOut", Name = "LogOut")]
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToRoute("Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("ExternalLogin")]
        public ActionResult ExternalLogin(string provider)
        {
            string returnUrl = Url.RouteUrl("Home");
            return new ChallengeResult(provider, Url.RouteUrl("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        [Route("ExternalLoginCallback", Name = "ExternalLoginCallback")]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToRoute("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if (returnUrl != null)
                        return Redirect(returnUrl);
                    return RedirectToRoute("Home");
                case SignInStatus.Failure:
                default:
                    if (ModelState.IsValid)
                    {
                        var identity = new User { UserName = loginInfo.DefaultUserName, Email = loginInfo.Email };
                        var newUser = await UserManager.CreateAsync(identity);
                        if (newUser.Succeeded)
                        {
                            newUser = await UserManager.AddLoginAsync(identity.Id, loginInfo.Login);
                            if (newUser.Succeeded)
                            {
                                await SignInManager.SignInAsync(identity, isPersistent: false, rememberBrowser: false);
                                return RedirectToLocal(returnUrl);
                            }
                        }
                    }

                    ViewBag.ReturnUrl = returnUrl;
                    return RedirectToRoute("Login");
            }
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
    }
}