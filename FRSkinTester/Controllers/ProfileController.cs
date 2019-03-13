using FRSkinTester.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace FRSkinTester.Controllers
{
    public class ProfileController : BaseController
    {
        [Authorize]
        [Route("Profile")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Profile/{username}")]
        public ActionResult Index(string username)
        {
            using (var ctx = new DataContext())
            {
                var user = ctx.Users.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower());
                if(user == null)
                {
                    TempData["Error"] = $"No user with username '{username}' could be found";
                    return RedirectToRoute("Home");
                }
                else if (user.Privacy == Infrastructure.DataModels.Privacy.HideAll)
                {
                    TempData["Error"] = "This user's profile is set to private";
                    return RedirectToRoute("Home");
                }
                return View();
            }
        }
    }
}