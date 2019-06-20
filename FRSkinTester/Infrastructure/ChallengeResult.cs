using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;

namespace FRTools.Infrastructure
{
    public class ChallengeResult : HttpUnauthorizedResult
    {
        public const string _xsrfKey = "FRTools%!3@18%!*8";

        public ChallengeResult(string provider, string redirectUri)
            : this(provider, redirectUri, null)
        { }

        public ChallengeResult(string provider, string redirectUri, string userId)
        {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
        }

        public string LoginProvider { get; set; }
        public string RedirectUri { get; set; }
        public string UserId { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            if (UserId != null)
                properties.Dictionary[_xsrfKey] = UserId;

            var owin = context.HttpContext.GetOwinContext();
            owin.Authentication.Challenge(properties, LoginProvider);
        }
    }
}