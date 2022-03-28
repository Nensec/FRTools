﻿using FRTools.Data;
using FRTools.Data.DataModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    public class BaseController : Controller
    {
        private User _loggedInUser;

        protected ActionResult RedirectToLocal(string returnUrl) => Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : (ActionResult)RedirectToAction("Index", "Home");

        protected DataContext DataContext { get; } = new DataContext();
        public User LoggedInUser => _loggedInUser ?? (_loggedInUser = GetLoggedInUser());

        [Route("robots.txt")]
        public ContentResult DynamicRobotsFile()
        {
            var content = new StringBuilder();
            content.AppendLine("user-agent: *");
            content.AppendLine("Allow: /");
            content.AppendLine("Disallow: /newsreader/view/*");
            content.AppendLine("Disallow: /newsreader/viewdeleted/*");
            return Content(content.ToString(), "text/plain", Encoding.UTF8);
        }

        private User GetLoggedInUser()
        {
            if (Request.IsAuthenticated)
            {
                var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
                return DataContext.Users.FirstOrDefault(x => x.Id == userid);
            }

            return null;
        }


        protected override void EndExecute(System.IAsyncResult asyncResult)
        {
            base.EndExecute(asyncResult);
            DataContext.Dispose();
        }

        protected void AddErrorNotification(string error)
        {
            if (error != null)
            {
                if (!string.IsNullOrEmpty(TempData["Error"] as string))
                    TempData["Error"] += "<br/>";
                TempData["Error"] += error;
            }
        }
        protected void AddWarningNotification(string info)
        {
            if (info != null)
            {
                if (!string.IsNullOrEmpty(TempData["Warning"] as string))
                    TempData["Warning"] += "<br/>";
                TempData["Warning"] += info;
            }
        }

        protected void AddInfoNotification(string info)
        {
            if (info != null)
            {
                if (!string.IsNullOrEmpty(TempData["Info"] as string))
                    TempData["Info"] += "<br/>";
                TempData["Info"] += info;
            }
        }

        protected void AddSuccessNotification(string success)
        {
            if (success != null)
            {
                if (!string.IsNullOrEmpty(TempData["Success"] as string))
                    TempData["Success"] += "<br/>";
                TempData["Success"] += success;
            }
        }
    }
}