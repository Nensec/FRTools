using System.Web.Mvc;

namespace FRSkinTester.Controllers
{
    public class HomeController : BaseController
    {
        [Route(Name = "Home")]
        public ActionResult Index() => View();
    }
}