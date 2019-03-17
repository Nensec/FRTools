﻿using FRSkinTester.Infrastructure;
using FRSkinTester.Infrastructure.DataModels;
using FRSkinTester.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FRSkinTester.Controllers
{
    public class ProfileController : BaseController
    {
        [Authorize]
        [Route("Profile", Name = "SelfProfile")]
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
                TempData["Info"] = "You are looking at your own profile, this will show all data regardless of your customization settings";
                return View(vm);
            }
        }

        [Route("Profile/{username}", Name = "Profile")]
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
    }
}