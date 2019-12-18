using FRTools.Data.DataModels;
using FRTools.Web.Infrastructure.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FRTools.Web.Infrastructure
{
    public class MustHaveLoginProviderAttribute : ActionFilterAttribute
    {
        private readonly string _provider;
        private readonly string _redirectRoute;

        public MustHaveLoginProviderAttribute(string provider, string redirectRoute)
        {
            _provider = provider;
            _redirectRoute = redirectRoute;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAuthenticated)            
                ErrorResult(filterContext);            
            else
            {
                var owin = HttpContext.Current.GetOwinContext();
                var userManager = owin.GetUserManager<UserManager<User, int>>();
                var logins = userManager.GetLogins(HttpContext.Current.User.Identity.GetUserId<int>());
                if (!logins.Any(x => x.LoginProvider.ToLower() == _provider.ToLower()))
                    ErrorResult(filterContext);                
            }
            base.OnActionExecuting(filterContext);
        }

        private void ErrorResult(ActionExecutingContext filterContext)
        {
            filterContext.Controller.TempData["Error"] = $"You need to have a <b>{_provider}</b> login attached to your account to access to this resource";
            filterContext.Result = new RedirectToRouteResult(_redirectRoute, null);
        }
    }
}