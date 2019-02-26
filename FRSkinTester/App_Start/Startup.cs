using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Owin.Security.Twitter;
using Owin;
using Owin.Security.Providers.DeviantArt;
using Owin.Security.Providers.Google;
using Owin.Security.Providers.Tumblr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;

[assembly: OwinStartup(typeof(FRSkinTester.App_Start.Startup))]
namespace FRSkinTester.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            app.UseGoogleAuthentication(new GoogleAuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["GoogleClientId"],
                ClientSecret = ConfigurationManager.AppSettings["GoogleSecret"]
            });

            app.UseTwitterAuthentication(new TwitterAuthenticationOptions
            {
                ConsumerKey = ConfigurationManager.AppSettings["TwitterClientId"],
                ConsumerSecret = ConfigurationManager.AppSettings["TwitterSecret"]
            });

            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountAuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["MicrosoftClientId"],
                ClientSecret = ConfigurationManager.AppSettings["MicrosoftSecret"]
            });

            app.UseTumblrAuthentication(new TumblrAuthenticationOptions
            {
                AppKey = ConfigurationManager.AppSettings["TumblrClientId"],
                AppSecret = ConfigurationManager.AppSettings["TumblrSecret"]
            });

            app.UseDeviantArtAuthentication(new DeviantArtAuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["DeviantArtClientId"],
                ClientSecret = ConfigurationManager.AppSettings["DeviantArtSecret"]
            });
        }
    }
}