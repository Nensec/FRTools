﻿using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Tools.SkinTester;
using FRTools.Web.Infrastructure;
using FRTools.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("skintester")]
    public class SkinController : BaseController
    {
        static MemoryCache _sceneCache = new MemoryCache("scenes");
        static MemoryCache _familiarCache = new MemoryCache("familiars");

        public SkinController()
        {
            ViewBag.Logo = "/Content/frskintester.svg";
            ViewBag.PngLogo = "/Content/frskintester.png";
        }

        [Route(Name = "SkinTesterHome")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("preview", Name = "PreviewHome")]
        public ActionResult PreviewHome()
        {
            return View();
        }

        [Route("preview/{skinId}", Name = "Preview")]
        public async Task<ActionResult> Preview(PreviewModelGet model)
        {
            var skin = DataContext.Skins.Include(x => x.Creator.FRUser).FirstOrDefault(x => x.GeneratedId == model.SkinId);
            if (skin == null)
            {
                AddErrorNotification("Skin not found");
                return RedirectToRoute("Home");
            }
            if (skin.Coverage == null)
                await UpdateCoverage(skin, DataContext);
            try
            {
                var isOwn = Request.IsAuthenticated && skin.Creator?.Id == HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
                if (isOwn && skin.Visibility == SkinVisiblity.HideEverywhere)
                    AddInfoNotification("Notice: You have set this skin to be hidden everywhere, only people who you share the link to this skin with can see this skin.");

                return View(new PreviewModelViewModel
                {
                    Title = skin.Title,
                    Description = skin.Description,
                    SkinId = model.SkinId,
                    PreviewUrl = (await SkinTester.GenerateOrFetchDummyPreview(skin.GeneratedId, skin.Version)).Urls[0],
                    Coverage = skin.Coverage,
                    Creator = skin.Creator,
                    DragonType = (DragonType)skin.DragonType,
                    Gender = (Gender)skin.GenderType,
                    Visibility = skin.Visibility,
                    Version = skin.Version
                });
            }
            catch (FileNotFoundException)
            {
                AddErrorNotification("Skin not found");
                return RedirectToRoute("Home");
            }
        }

        [HttpPost]
        [Route("preview/{skinId}", Name = "PreviewPost")]
        public async Task<ActionResult> Preview(PreviewModelPost model)
        {
            if (!ModelState.IsValid)
                return RedirectToRoute("Preview", new { model.SkinId });

            PreviewResult result = null;
            if (model.DragonId != null)
                result = await SkinTester.GenerateOrFetchPreview(model.SkinId, model.DragonId.Value, model.SwapSilhouette, model.Force);
            else if (!string.IsNullOrWhiteSpace(model.ScryerUrl))
                result = await SkinTester.GenerateOrFetchPreview(model.SkinId, model.ScryerUrl, model.Force);
            else if (!string.IsNullOrWhiteSpace(model.DressingRoomUrl))
                result = await SkinTester.GenerateOrFetchPreview(model.SkinId, model.DressingRoomUrl, model.Force);

            if (result == null || !result.Success)
            {
                AddErrorNotification(result?.GetHtmlErrorMessage);
                return RedirectToRoute("Preview", new { model.SkinId });
            }

            await SavePreviewStatistics(result);

            var scenes = _sceneCache.Cast<KeyValuePair<string, object>>().Select(x => x.Value).Cast<FRItem>().ToList();
            var familiars = _familiarCache.Cast<KeyValuePair<string, object>>().Select(x => x.Value).Cast<FRItem>().ToList();

            if (!scenes.Any())
            {
                scenes = DataContext.FRItems.Where(x => x.ItemCategory == FRItemCategory.Trinket && x.ItemType == "Scene").ToList();
                scenes.ForEach(x => _sceneCache.Add($"{x.FRId}", x, DateTimeOffset.UtcNow.AddMinutes(15)));
            }

            if(!familiars.Any())
            {
                familiars = DataContext.FRItems.Where(x => x.ItemCategory == FRItemCategory.Familiar).ToList();
                familiars.ForEach(x => _familiarCache.Add($"{x.FRId}", x, DateTimeOffset.UtcNow.AddMinutes(15)));
            }

            return View("PreviewResult", new PreviewModelPostViewModel
            {
                SkinId = model.SkinId,
                Result = result,
                Dragon = result.Dragon,
                Scenes = scenes,
                Familiars = familiars
            });
        }

        private async Task SavePreviewStatistics(PreviewResult previewResult)
        {
            var loggedInUserId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();

            var user = DataContext.Users.Find(loggedInUserId);

            Func<string, bool> existing;
            if (user == null)
                existing = url => !DataContext.Previews.Where(x => x.Skin.Id == previewResult.Skin.Id).Any(x => x.PreviewImage == url);
            else
                existing = url => !DataContext.Previews.Where(x => x.Skin.Id == previewResult.Skin.Id).Any(x => x.Requestor.Id == user.Id && x.PreviewImage == url);

            foreach (var url in previewResult.Urls.Where(existing))
            {
                DataContext.Previews.Add(new Preview
                {
                    Skin = DataContext.Skins.Find(previewResult.Skin.Id),
                    DragonId = previewResult.Dragon.FRDragonId,
                    ScryerUrl = previewResult.DragonUrl,
                    PreviewImage = url,
                    DragonData = previewResult.Dragon.ToString(),
                    PreviewTime = DateTime.UtcNow,
                    Requestor = DataContext.Users.Find(loggedInUserId),
                    Version = previewResult.Skin.Version
                });
            }

            await DataContext.SaveChangesAsync();
        }

        [Route("upload", Name = "Upload")]
        public ActionResult Upload() => View(new UploadModelPost());

        [Route("upload", Name = "UploadPost")]
        [HttpPost]
        public async Task<ActionResult> Upload(UploadModelPost model)
        {
            var azureImageService = new AzureFileService();

            var randomizedId = CodeHelpers.GenerateId(5, DataContext.Skins.Select(x => x.GeneratedId).ToList());
            var secretKey = CodeHelpers.GenerateId(7);
            Bitmap skinImage = null;
            try
            {
                skinImage = (Bitmap)Image.FromStream(model.Skin.InputStream);
                if (skinImage.Width != 350 || skinImage.Height != 350)
                {
                    AddErrorNotification("Image needs to be 350px x 350px. Just like FR.");
                    return View();
                }
            }
            catch
            {
                AddErrorNotification("Upload is not a valid png image");
                return View();
            }
            try
            {
                var fixPixelFormat = SkinTester.FixPixelFormat(skinImage);
                if (fixPixelFormat != null)
                    skinImage = fixPixelFormat;

                model.Skin.InputStream.Position = 0;
                var url = await azureImageService.WriteFile($@"skins\{randomizedId}.png", model.Skin.InputStream);

                Bitmap dragonImage = null;
                using (var client = new WebClient())
                {
                    var dwagonImageBytes = client.DownloadDataTaskAsync(string.Format(FRHelpers.DressingRoomDummyUrl, (int)model.DragonType, (int)model.Gender));
                    try
                    {
                        using (var memStream = new MemoryStream(await dwagonImageBytes, false))
                            dragonImage = (Bitmap)Image.FromStream(memStream);
                    }
                    catch
                    {
                    }
                }

                var skin = new Skin
                {
                    GeneratedId = randomizedId,
                    SecretKey = secretKey,
                    Title = model.Title,
                    Description = model.Description,
                    DragonType = (int)model.DragonType,
                    GenderType = (int)model.Gender,
                    Coverage = GetCoveragePercentage(skinImage, dragonImage)
                };
                skinImage.Dispose();

                if (Request.IsAuthenticated)
                {
                    var userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
                    skin.Creator = DataContext.Users.FirstOrDefault(x => x.Id == userId);
                    skin.Visibility = skin.Creator.ProfileSettings.DefaultSkinsArePublic ? skin.Creator.ProfileSettings.DefaultShowSkinsInBrowse ? SkinVisiblity.Visible : SkinVisiblity.HideFromBrowse : SkinVisiblity.HideEverywhere;
                }

                DataContext.Skins.Add(skin);
                await DataContext.SaveChangesAsync();
                return View("UploadResult", new UploadModelPostViewModel
                {
                    SkinId = randomizedId,
                    SecretKey = secretKey,
                    PreviewImageUrl = (await SkinTester.GenerateOrFetchDummyPreview(randomizedId, skin.Version)).Urls[0]
                });
            }
            catch
            {
                AddErrorNotification("Something went wrong uploading");
                return View();
            }
        }


        [Route("manage/skin/{skinId}/{secretKey}", Name = "Manage")]
        public async Task<ActionResult> Manage(ManageModelGet model)
        {
            var skin = DataContext.Skins.Include(x => x.Previews).FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
            if (skin == null)
            {
                AddErrorNotification("Skin not found or secret invalid");
                return RedirectToRoute("Home");
            }
            else
            {
                if (skin.Creator != null)
                {
                    if (!Request.IsAuthenticated)
                    {
                        AddErrorNotification("This skin is linked to an account, please log in to manage this skin.");
                        return RedirectToRoute("Home");
                    }
                    else if (skin.Creator.Id != HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>())
                    {
                        AddErrorNotification("This skin is linked to a different account than the one that is logged in, you can only manage your own skins.");
                        return RedirectToRoute("Home");
                    }
                }

                if (skin.Coverage == null)
                    await UpdateCoverage(skin, DataContext);
                return View(new ManageModelViewModel
                {
                    Skin = skin,
                    PreviewImageUrl = (await SkinTester.GenerateOrFetchDummyPreview(skin.GeneratedId, skin.Version)).Urls[0],
                    PreviewUrl = Url.RouteUrl("Preview", new { SkinId = skin.GeneratedId }, "https"),
                });
            }
        }

        [Route("manage/skin", Name = "ManagePost")]
        [HttpPost]
        public async Task<ActionResult> Manage(ManageModelPost model)
        {
            var skin = DataContext.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
            if (skin == null)
            {
                AddErrorNotification("Skin not found or secret invalid");
                return RedirectToRoute("Home");
            }
            else
            {
                if (skin.Creator != null)
                {
                    if (!Request.IsAuthenticated)
                    {
                        AddErrorNotification("This skin is linked to an account, please log in to manage this skin.");
                        return RedirectToRoute("Home");
                    }
                    else if (skin.Creator.Id != HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>())
                    {
                        AddErrorNotification("This skin is linked to a different account than the one that is logged in, you can only manage your own or unclaimed skins.");
                        return RedirectToRoute("Home");
                    }
                }

                if (skin.GenderType != (int)model.Gender || skin.DragonType != (int)model.DragonType)
                {
                    var azureImageService = new AzureFileService();
                    await azureImageService.DeleteFile($@"previews\{model.SkinId}\{(skin.Version == 1 ? "" : $@"{skin.Version}\")}preview.png");
                }

                skin.Title = model.Title;
                skin.Description = model.Description;
                skin.GenderType = (int)model.Gender;
                skin.DragonType = (int)model.DragonType;
                skin.Visibility = model.Visibility;

                await DataContext.SaveChangesAsync();

                AddSuccessNotification("Changes have been saved!");
                return RedirectToRoute("Manage", new { model.SkinId, model.SecretKey });
            }
        }

        [Route("manage", Name = "ManageSkins")]
        public ActionResult ManageSkins(PaginationModel pagination)
        {
            var model = new ManageSkinsViewModel
            {
                Pagination = new PaginationModel("ManageSkins", pagination.Page, pagination.PageSize)
                {
                    ValidPageSizes = new[] { 6, 12, 18, 24, 30 }
                }
            };

            model.Skins = LoggedInUser.Skins.OrderByDescending(x => x.Id).Skip(model.Pagination.PageSize * (model.Pagination.Page - 1)).Take(model.Pagination.PageSize).ToList();
            model.Pagination.TotalItems = LoggedInUser.Skins.Count();

            return View(model);
        }

        [Route("manage/skin/update", Name = "UpdateSkinPost")]
        [HttpPost]
        public async Task<ActionResult> UpdateSkin(UpdateSkinPost model)
        {
            var azureImageService = new AzureFileService();

            var skin = DataContext.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
            if (skin == null)
            {
                AddErrorNotification("Skin not found or secret invalid");
                return RedirectToRoute("Home");
            }
            else
            {
                if (skin.Creator != null)
                {
                    if (!Request.IsAuthenticated)
                    {
                        AddErrorNotification("This skin is linked to an account, please log in to update this skin.");
                        return RedirectToRoute("Home");
                    }
                    else if (skin.Creator.Id != HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>())
                    {
                        AddErrorNotification("This skin is linked to a different account than the one that is logged in, you can only update your own or unclaimed skins.");
                        return RedirectToRoute("Home");
                    }
                }
            }

            Bitmap skinImage = null;
            try
            {
                skinImage = (Bitmap)Image.FromStream(model.Skin.InputStream);
                if (skinImage.Width != 350 || skinImage.Height != 350)
                {
                    AddErrorNotification("Image needs to be 350px x 350px. Just like FR.");
                    return RedirectToRoute("Manage", new { model.SkinId, model.SecretKey });
                }

            }
            catch
            {
                AddErrorNotification("Upload is not a valid png image");
                return RedirectToRoute("Manage", new { model.SkinId, model.SecretKey });
            }

            try
            {
                model.Skin.InputStream.Position = 0;
                var url = await azureImageService.WriteFile($@"skins\{model.SkinId}.png", model.Skin.InputStream);

                Bitmap dragonImage = null;
                using (var client = new WebClient())
                {
                    var dwagonImageBytes = client.DownloadDataTaskAsync(string.Format(FRHelpers.DressingRoomDummyUrl, skin.DragonType, (int)skin.GenderType));
                    try
                    {
                        using (var memStream = new MemoryStream(await dwagonImageBytes, false))
                            dragonImage = (Bitmap)Image.FromStream(memStream);
                    }
                    catch
                    {
                    }
                }

                skin.Version += 1;
                skin.Coverage = GetCoveragePercentage(skinImage, dragonImage);

                skinImage.Dispose();

                if (Request.IsAuthenticated)
                {
                    var userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
                    skin.Creator = DataContext.Users.FirstOrDefault(x => x.Id == userId);
                }

                await DataContext.SaveChangesAsync();
                AddSuccessNotification($"Skin succesfully updated to version <b>v{skin.Version}</b>!");
                return RedirectToRoute("Manage", new { model.SkinId, model.SecretKey });
            }
            catch
            {
                AddErrorNotification("Something went wrong uploading");
                return RedirectToRoute("Manage", new { model.SkinId, model.SecretKey });
            }
        }

        [Route("manage/skin/delete", Name = "Delete")]
        public async Task<ActionResult> Delete(DeleteSkinPost model)
        {
            var skin = DataContext.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
            if (skin == null)
            {
                AddErrorNotification("Skin not found or secret invalid");
                return RedirectToRoute("Home");
            }
            else
            {
                var azureImageService = new AzureFileService();
                await azureImageService.DeleteFile($@"skins\{model.SkinId}.png");
                skin.Previews.Clear();
                DataContext.Skins.Remove(skin);
                await DataContext.SaveChangesAsync();
            }

            return View();
        }


        [Route("browse", Name = "Browse")]
        public async Task<ActionResult> Browse(PaginationModel pagination, [ModelBinder(typeof(CommaSeparatedValueModelBinder))] BrowseFilterModel filter)
        {
            var model = new BrowseViewModel { Filter = filter };
            model.Pagination = new PaginationModel("Browse", pagination.Page, pagination.PageSize);

            var query = DataContext.Skins
                .Where(x => x.Visibility == SkinVisiblity.Visible)
                .Where(x => filter.DragonTypes.Contains((DragonType)x.DragonType))
                .Where(x => filter.Genders.Contains((Gender)x.GenderType))
                .Where(x => filter.SkinTypes.Contains((BrowseFilterModel.SkinType)(x.Coverage >= 31 ? 1 : 0)));

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => x.Title.Contains(filter.Name));

            model.Pagination.TotalItems = query.Count();

            query = query.OrderByDescending(x => x.Id).Skip(model.Pagination.PageSize * (model.Pagination.Page - 1)).Take(model.Pagination.PageSize);

            model.Results = query.Select(x => new PreviewModelViewModel
            {
                Title = x.Title,
                Description = x.Description,
                SkinId = x.GeneratedId,
                Coverage = x.Coverage,
                Creator = x.Creator,
                DragonType = (DragonType)x.DragonType,
                Gender = (Gender)x.GenderType,
                Version = x.Version
            }).ToList();

            foreach (var result in model.Results)
                result.PreviewUrl = (await SkinTester.GenerateOrFetchDummyPreview(result.SkinId, result.Version)).Urls[0];

            return View(model);
        }

        [Route("manage/link", Name = "LinkExisting")]
        public ActionResult LinkExistingSkin() => View();

        [HttpPost]
        [Route("manage/link", Name = "LinkExistingPost")]
        public ActionResult LinkExistingSkin(ClaimSkinPostViewModel model)
        {
            var skin = DataContext.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
            if (skin == null)
            {
                AddErrorNotification("Skin not found or secret invalid");
                return View();
            }
            int userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();

            if (skin.Creator != null)
            {
                AddErrorNotification("This skin is already linked to an account, skins can only be claimed by a single account.");
                return View();
            }

            skin.Creator = DataContext.Users.Find(userId);
            DataContext.SaveChanges();

            AddSuccessNotification($"Succesfully linked skin '{model.SkinId}' to your account!");
            return RedirectToRoute("ManageAccount");
        }


        [Route("manage/preview/unlink", Name = "UnlinkPreview")]
        [HttpPost]
        public ActionResult UnlinkPreview(int previewId)
        {
            var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();
            var user = DataContext.Users.Include(x => x.Previews.Select(p => p.Skin)).FirstOrDefault(x => x.Id == userid);
            var preview = user.Previews.FirstOrDefault(x => x.Id == previewId);
            if (preview != null)
            {
                preview.Requestor = null;
                DataContext.SaveChanges();
            }

            return Json(new { previewId });
        }

        private double? GetCoveragePercentage(Bitmap skinImage, Bitmap dragonImage)
        {
            if (dragonImage == null)
                return null;

            var alphaSum = 0d;
            var pixelCount = 0d;

            for (var x = 0; x < 350; x++)
                for (var y = 0; y < 350; y++)
                {
                    var skinPixel = skinImage.GetPixel(x, y);
                    var dragonPixel = dragonImage.GetPixel(x, y);
                    if (dragonPixel.A > 95)
                    {
                        if (skinPixel.A > 0)
                            alphaSum += skinPixel.A;
                        pixelCount++;
                    }
                }

            return Math.Round(alphaSum / pixelCount / 255 * 100, 2);
        }

        private async Task UpdateCoverage(Skin skin, DataContext ctx)
        {
            Bitmap skinImage, dummyImage;

            var azureImageService = new AzureFileService();
            using (var stream = await azureImageService.GetFile($@"skins\{skin.GeneratedId}.png"))
                skinImage = (Bitmap)Image.FromStream(stream);

            try
            {
                dummyImage = await FRHelpers.GetDragonBaseImage(string.Format(FRHelpers.DressingRoomDummyUrl, skin.DragonType, skin.GenderType));

                skin.Coverage = GetCoveragePercentage(skinImage, dummyImage);
                skinImage.Dispose();
                dummyImage.Dispose();
                await ctx.SaveChangesAsync();
            }
            catch
            {
                // TODO: Something went wrong
            }
        }

        [Route("dummy", Name = "DummyPreview")]
        public async Task<ActionResult> GenerateOrFetchDummyPreview(string skinid, int version)
        {
            var result = await SkinTester.GenerateOrFetchDummyPreview(skinid, version);
            if (Request.Headers["If-Modified-Since"] != null && result.Cached)
                return new HttpStatusCodeResult(HttpStatusCode.NotModified, "Not Modified");
            else
            {
                using (var client = new WebClient())
                {
                    return File(await client.DownloadDataTaskAsync(ConfigurationManager.AppSettings["CDNBasePath"] + result.Urls[0]), "image/png");
                }
            }
        }
    }
}