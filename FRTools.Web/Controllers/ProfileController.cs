using FRTools.Data;
using FRTools.Data.DataModels;
using FRTools.Web.Models;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("profile")]
    [Authorize]
    public class ProfileController : BaseController
    {
        [Route(Name = "SelfProfile")]
        public ActionResult Index()
        {
            var vm = new ViewProfileViewModel
            {
                User = LoggedInUser,
                Previews = LoggedInUser.Previews.ToList(),
                Skins = LoggedInUser.Skins.ToList(),
                Pinglists = LoggedInUser.Pinglists.ToList(),
                IsOwn = true,
            };
            AddInfoNotification($"You are looking at your own profile, you will see everything regardless of settings. To see your profile as others see it visit <a href=\"{Url.RouteUrl("Profile", new { username = vm.User.UserName })}\"><b>this link</b></a>. To edit your profile, go to the edit page <a href=\"{Url.RouteUrl("EditProfile")}\"><b>here</b></a>.");
            return View(vm);
        }

        [Route("{*username}", Name = "Profile")]
        [AllowAnonymous]
        public ActionResult Index(string username)
        {
            var user = DataContext.Users.Include(x => x.FRUser).Include(x => x.Previews.Select(p => p.Skin)).FirstOrDefault(x => x.UserName.ToLower() == username.ToLower());
            if (user == null)
            {
                AddErrorNotification($"No user with username '{username}' could be found");
                return RedirectToRoute("Home");
            }
            else if (!user.ProfileSettings.PublicProfile)
            {
                AddErrorNotification("This user's profile is set to private");
                return RedirectToRoute("Home");
            }
            var vm = new ViewProfileViewModel { User = user };
            if (user.ProfileSettings.ShowPreviewsOnProfile)
                vm.Previews = user.Previews.ToList();
            if (user.ProfileSettings.ShowSkinsOnProfile)
            {
                vm.Skins = user.Skins.Where(x => x.Visibility == SkinVisiblity.Visible || x.Visibility == SkinVisiblity.HideFromBrowse).ToList();
            }
            if (LoggedInUser != null && user.ProfileSettings.ShowPingListsOnProfile)
            {
                vm.Pinglists = user.Pinglists.Where(x => x.IsPublic || x.Entries.Any(e => e.FRUser.User?.Id == LoggedInUser.Id)).ToList();
            }
            return View(vm);
        }

        [Route("edit", Name = "EditProfile")]
        public ActionResult Edit()
        {
            var vm = new EditProfileViewModel
            {
                Username = LoggedInUser.UserName,
                DefaultShowSkinsInBrowse = LoggedInUser.ProfileSettings.DefaultShowSkinsInBrowse,
                DefaultSkinsArePublic = LoggedInUser.ProfileSettings.DefaultSkinsArePublic,
                ProfileBio = LoggedInUser.ProfileSettings.ProfileBio,
                PublicProfile = LoggedInUser.ProfileSettings.PublicProfile,
                ShowPingListsOnProfile = LoggedInUser.ProfileSettings.ShowPingListsOnProfile,
                ShowFRLinkStatus = LoggedInUser.ProfileSettings.ShowFRLinkStatus,
                ShowPreviewsOnProfile = LoggedInUser.ProfileSettings.ShowPreviewsOnProfile,
                ShowSkinsOnProfile = LoggedInUser.ProfileSettings.ShowSkinsOnProfile,
                ShowAds = LoggedInUser.ProfileSettings.ShowAds,
                DefaultAdvancedCoverageSkinOpacity = LoggedInUser.ProfileSettings.DefaultAdvancedCoverageSkinOpacity,
                DefaultAdvancedCoverageOverlayColor = LoggedInUser.ProfileSettings.DefaultAdvancedCoverageOverlayColor,
                DefaultAdvancedCoverageBackgroundColor = LoggedInUser.ProfileSettings.DefaultAdvancedCoverageBackgroundColor,
                DefaultAdvancedCoverageDummyOpacity = LoggedInUser.ProfileSettings.DefaultAdvancedCoverageDummyOpacity,
                DefaultAdvancedCoveragePercentagePrecision = LoggedInUser.ProfileSettings.DefaultAdvancedCoveragePercentagePrecision
            };
            return View(vm);
        }

        private readonly string[] _blacklist = new[] { "edit", "manage" };

        [HttpPost]
        [Route("edit", Name = "EditProfilePost")]
        public ActionResult Edit(EditProfileViewModel model)
        {
            try
            {
                LoggedInUser.UserName = string.IsNullOrWhiteSpace(model.Username) || _blacklist.Contains(model.Username.ToLower()) ? LoggedInUser.UserName : model.Username;
                LoggedInUser.ProfileSettings.PublicProfile = model.PublicProfile;
                LoggedInUser.ProfileSettings.ProfileBio = model.ProfileBio;
                LoggedInUser.ProfileSettings.DefaultShowSkinsInBrowse = model.DefaultShowSkinsInBrowse;
                LoggedInUser.ProfileSettings.DefaultSkinsArePublic = model.DefaultSkinsArePublic;
                LoggedInUser.ProfileSettings.ShowFRLinkStatus = model.ShowFRLinkStatus;
                LoggedInUser.ProfileSettings.ShowPingListsOnProfile = model.ShowPingListsOnProfile;
                LoggedInUser.ProfileSettings.ShowPreviewsOnProfile = model.ShowPreviewsOnProfile;
                LoggedInUser.ProfileSettings.ShowSkinsOnProfile = model.ShowSkinsOnProfile;
                LoggedInUser.ProfileSettings.ShowAds = model.ShowAds;
                LoggedInUser.ProfileSettings.DefaultAdvancedCoverageBackgroundColor = model.DefaultAdvancedCoverageBackgroundColor;
                LoggedInUser.ProfileSettings.DefaultAdvancedCoverageDummyOpacity = model.DefaultAdvancedCoverageDummyOpacity;
                LoggedInUser.ProfileSettings.DefaultAdvancedCoverageOverlayColor = model.DefaultAdvancedCoverageOverlayColor;
                LoggedInUser.ProfileSettings.DefaultAdvancedCoverageSkinOpacity = model.DefaultAdvancedCoverageSkinOpacity;
                LoggedInUser.ProfileSettings.DefaultAdvancedCoveragePercentagePrecision = model.DefaultAdvancedCoveragePercentagePrecision;
                DataContext.SaveChanges();
                AddSuccessNotification("Your profile has been updated!");
            }
            catch (Exception ex)
            {
                var actualException = ex;
                while (actualException.InnerException != null)
                    actualException = actualException.InnerException;

                if (actualException is SqlException sqlEx && sqlEx.Number == 2601)                
                    AddErrorNotification("That username is already taken, please pick a different one");                
                else
                    AddErrorNotification("Something went wrong with your request");
                return View(model);
            }
            return RedirectToRoute("SelfProfile");
        }

        [HttpPost]
        [Route("savesetting", Name = "SaveProfileSetting")]
        public ActionResult SaveSetting(string key, string value)
        {
            LoggedInUser.ProfileSettings[key] = value;
            DataContext.SaveChanges();
            return Json(new { success = true });
        }
    }
}