using System.Web.Mvc;

namespace FRTools.Controllers
{
    public class HomeController : BaseController
    {
        [Route(Name = "Home")]
        public ActionResult Index() => View();
    }
}