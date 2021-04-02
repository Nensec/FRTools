using FRTools.Data.DataModels;
using FRTools.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private SignInManager<User, int> _signInManager;
        private UserManager<User, int> _userManager;

        public SignInManager<User, int> SignInManager => _signInManager ?? (_signInManager = HttpContext.GetOwinContext().Get<SignInManager<User, int>>());

        public UserManager<User, int> UserManager => _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<User, int>>());


        [Route("users", Name = "AdminUsers")]
        public ActionResult Users()
        {
            var users = DataContext.Users.ToList();
            var vm = new AdminUsersViewModel
            {
                Users = users
            };
            return View(vm);
        }

        [Route("users/{userId}/impersonate", Name = "AdminImpersonate")]
        public async Task<ActionResult> Impersonate(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if(user == null)
            {
                AddErrorNotification($"User not found: {userId}");
                return RedirectToRoute("AdminUsers");
            }

            await SignInManager.SignInAsync(user, false, false);
            return RedirectToRoute("Home");
        }
    }
}