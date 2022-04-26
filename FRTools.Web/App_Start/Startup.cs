using FRTools.Data;
using FRTools.Data.DataModels;
using FRTools.Web.Infrastructure.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Owin.Security.Twitter;
using Owin;
using Owin.Security.Providers.DeviantArt;
using Owin.Security.Providers.Discord;
using Owin.Security.Providers.Google;
using Owin.Security.Providers.Tumblr;
using System;
using System.Configuration;
using System.Linq;

[assembly: OwinStartup(typeof(FRTools.App_Start.Startup))]
namespace FRTools.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new DataContext());
            app.CreatePerOwinContext<UserManager<User, int>>((o, c) =>
            {
                var userManager = new UserManager<User, int>(new UserStore(c.Get<DataContext>()));
                userManager.UserValidator = new UserValidator<User, int>(userManager) { AllowOnlyAlphanumericUserNames = false };
                return userManager;
            });
            app.CreatePerOwinContext<SignInManager<User, int>>((o, c) => new SignInManager<User, int>(c.GetUserManager<UserManager<User, int>>(), c.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/account/login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManager<User, int>, User, int>(
                        TimeSpan.FromMinutes(30),
                        async (manager, user) => await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie),
                        user => user.GetUserId<int>())
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

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

            var discordOptions = new DiscordAuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["DiscordClientId"],
                ClientSecret = ConfigurationManager.AppSettings["DiscordSecret"],
            };

            discordOptions.Scope.Clear();
            discordOptions.Scope.Add("identify");
            discordOptions.Scope.Add("guilds");
            discordOptions.Scope.Add("guilds.members.read");
            app.UseDiscordAuthentication(discordOptions);

            IServiceCollection services = new ServiceCollection();
            services.AddLogging(x => x.AddApplicationInsights());

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            using (var ctx = new DataContext())
            {
                var activeJobs = ctx.Jobs.Where(x => x.Status == JobStatus.Running).ToList();
                activeJobs.ForEach(x => x.Status = JobStatus.Cancelled);
                ctx.SaveChanges();
            }

            using (var ctx = new DataContext())
            {
                var roles = ctx.Roles.ToList();
                if (!roles.Any(x => x.Name == "Admin"))
                    roles.Add(new Role { Name = "Admin" });
                ctx.SaveChanges();
            }
        }
    }
}