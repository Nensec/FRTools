using FRSkinTester.Infrastructure;
using FRSkinTester.Infrastructure.DataModels;
using FRSkinTester.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FRSkinTester.Controllers
{
    [RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        [Authorize]
        [Route(Name = "SelfProfile")]
        public ActionResult Index()
        {
            using (var ctx = new DataContext())
            {
                var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
                var user = ctx.Users.Include(x => x.Previews.Select(p => p.Skin)).FirstOrDefault(x => x.Id == userid);
                var vm = new ViewProfileViewModel
                {
                    User = user,
                    Previews = user.Previews.ToList(),
                    Skins = user.Skins.ToList(),
                    IsOwn = true

                };
                return View(vm);
            }
        }

        [Route("{username}", Name = "Profile")]
        public ActionResult Index(string username)
        {
            using (var ctx = new DataContext())
            {
                var user = ctx.Users.Include(x => x.Previews.Select(p => p.Skin)).FirstOrDefault(x => x.UserName.ToLower() == username.ToLower());
                if (user == null)
                {
                    TempData["Error"] = $"No user with username '{username}' could be found";
                    return RedirectToRoute("Home");
                }
                else if (user.Privacy == Privacy.HideAll)
                {
                    TempData["Error"] = "This user's profile is set to private";
                    return RedirectToRoute("Home");
                }
                var vm = new ViewProfileViewModel { User = user };
                if (!user.Privacy.HasFlag(Privacy.HidePreviews))
                    vm.Previews = user.Previews.ToList();
                if (!user.Privacy.HasFlag(Privacy.HideSkins))
                    vm.Skins = user.Skins.ToList();
                return View(vm);
            }
        }

        [Route("unlink", Name = "UnlinkPreview")]
        [HttpPost]
        public ActionResult UnlinkPreview(int previewId)
        {
            using (var ctx = new DataContext())
            {
                var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
                var user = ctx.Users.Include(x => x.Previews.Select(p => p.Skin)).FirstOrDefault(x => x.Id == userid);
                var preview = user.Previews.FirstOrDefault(x => x.Id == previewId);
                if (preview != null)
                {
                    preview.Requestor = null;
                    ctx.SaveChanges();
                }
            }
            return Json(new { previewId });
        }
    }
}