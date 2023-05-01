using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FRTools.Web.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Extensions.Logging;

namespace FRTools
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();

            var logger = Context.GetOwinContext().Get<LoggerWrapper>();

            if (logger.IsEnabled(LogLevel.Error))
                logger.LogError(ex, ex.Message);

#if DEBUG
            Trace.WriteLine(ex.Message);
#endif
        }
    }
}
