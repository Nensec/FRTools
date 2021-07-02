using System.Net;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    public class HomeController : BaseController
    {
        [Route(Name = "Home")]
        public ActionResult Index() => View();

        [Route("notfound", Name = "NotFound")]
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;
            return View("Error404");
        }

        [Route("error", Name = "Error")]
        public ActionResult Error()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;
            return View("Error500");
        }

        [Route("privacy", Name = "Privacy")]
        public ActionResult Privacy() => View();

        [Route("contact", Name = "Contact")]
        public ActionResult Contact() => View();
    }
}