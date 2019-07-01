using FRTools.Data.DataModels;
using FRTools.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FRTools.Data;

namespace FRTools.Controllers
{
    [RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        [Authorize]
        [Route(Name = "SelfProfile")]
        [Route("~/skintester/profile")] /* TODO: Delete this */
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

        [Route("{*username}", Name = "Profile")]
        [Route("~/skintester/profile/{*username}")] /* TODO: Delete this */
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
                    vm.Skins = user.Skins.Where(x => x.Visibility == SkinVisiblity.Visible || x.Visibility == SkinVisiblity.HideFromBrowse).ToList();
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