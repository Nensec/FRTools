using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    public class HomeController : BaseController
    {
        [Route(Name = "Home")]
        public ActionResult Index() => View();

        [Route("404", Name = "NotFound")]
        public ActionResult NotFound() => View();

        [Route("privacy", Name = "Privacy")]
        public ActionResult Privacy() => View();

        [Route("contact", Name = "Contact")]
        public ActionResult Contact() => View();
    }
}