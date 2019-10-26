using FRTools.Web.Infrastructure;
using FRTools.Data.DataModels;
using FRTools.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FRTools.Data;
using Color = FRTools.Data.Color;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("skintester")]
    public class SkinController : BaseController
    {
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
        [Route("~/preview")] /* TODO: Delete this */
        public ActionResult PreviewHome()
        {
            return View();
        }

        [Route("preview/{skinId}", Name = "Preview")]
        [Route("~/preview/{skinId}")] /* TODO: Delete this */
        public async Task<ActionResult> Preview(PreviewModelGet model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToRoute("Home");
                }
                if (skin.Coverage == null)
                    await UpdateCoverage(skin, ctx);
                try
                {
                    return View(new PreviewModelViewModel
                    {
                        Title = skin.Title,
                        Description = skin.Description,
                        SkinId = model.SkinId,
                        PreviewUrl = (await GenerateOrFetchPreview(model.SkinId, skin.Version, "preview", string.Format(FRHelpers.DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null)).Urls[0],
                        Coverage = skin.Coverage,
                        Creator = skin.Creator,
                        DragonType = (DragonType)skin.DragonType,
                        Gender = (Gender)skin.GenderType,
                        Visibility = skin.Visibility,
                        Version = skin.Version,
                        IsOwn = Request.IsAuthenticated && skin.Creator?.Id == HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>()
                    });
                }
                catch (FileNotFoundException)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToRoute("Home");
                }
            }
        }

        [HttpPost]
        [Route("preview/{skinId}", Name = "PreviewPost")]
        public async Task<ActionResult> Preview(PreviewModelPost model)
        {
            if (!ModelState.IsValid)
                return RedirectToRoute("Preview", new { model.SkinId });

            if (model.DragonId != null)
            {
                string dwagonUrl = FRHelpers.GetDragonImageUrlFromDragonId(model.DragonId.Value);

                if (dwagonUrl.StartsWith(".."))
                {
                    TempData["Error"] = $"<b>{model.DragonId}</b> appears to be an invalid dragon id";
                    return RedirectToRoute("Preview", new { model.SkinId });
                }

                return await GeneratePreview(model.SkinId, dwagonUrl, model.DragonId, model.Force);

            }
            else if (!string.IsNullOrWhiteSpace(model.ScryerUrl))
            {
                return await GeneratePreview(model.SkinId, model.ScryerUrl);
            }
            else if (!string.IsNullOrWhiteSpace(model.DressingRoomUrl))
            {
                return await GeneratePreview(model.SkinId, model.DressingRoomUrl, null, model.Force);
            }

            return RedirectToRoute("Preview", new { model.SkinId });
        }

        private async Task<ActionResult> GeneratePreview(string skinId, string dragonUrl, int? dragonId = null, bool force = false)
        {
            DragonCache dragon = null;
            bool isDressingRoomUrl = dragonUrl.Contains("/dgen/dressing-room");
            string dressingRoomUrl = isDressingRoomUrl ? dragonUrl : null;
            if (isDressingRoomUrl)
            {
                var apparelDragon = FRHelpers.ParseUrlForDragon(dragonUrl);
                if (!dragonUrl.Contains("/dgen/dressing-room/dummy"))
                {
                    dragonId = int.Parse(Regex.Match(dragonUrl, @"did=([\d]*)").Groups[1].Value);
                    dragon = FRHelpers.GetDragonFromDragonId(dragonId.Value);
                }
                else
                    dragon = FRHelpers.ParseUrlForDragon(dragonUrl);

                if (IsAncientBreed(dragon.DragonType))
                {
                    TempData["Error"] = $"Ancient breeds cannot wear apparal, how did you even get a dressing room link in here?";
                    return RedirectToRoute("Preview", new { skinId });
                }
                dragon.Apparel = apparelDragon.Apparel;
                if (dragon.GetApparel().Length == 0)
                {
                    TempData["Error"] = $"This dressing room URL contains no apparel";
                    return RedirectToRoute("Preview", new { skinId });
                }
            }
            else
                dragon = FRHelpers.ParseUrlForDragon(dragonUrl);

            if (dragon.Age == Age.Hatchling)
            {
                TempData["Error"] = $"Skins can only be previewed on adult dragons";
                return RedirectToRoute("Preview", new { skinId });
            }

            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == skinId);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToRoute("Home");
                }

                if (skin.DragonType != (int)dragon.DragonType)
                {
                    TempData["Error"] = $"This skin is meant for a <b>{(DragonType)skin.DragonType} {(Gender)skin.GenderType}</b>, the dragon you provided is a <b>{dragon.DragonType} {dragon.Gender}</b>";
                    return RedirectToRoute("Preview", new { skinId });
                }

                if (skin.GenderType != (int)dragon.Gender)
                {
                    TempData["Error"] = $"This skin is meant for a <b>{(Gender)skin.GenderType}</b>, the dragon you provided is a <b>{dragon.Gender}</b>";
                    return RedirectToRoute("Preview", new { skinId });
                }

                var previewResult = await GenerateOrFetchPreview(skinId, skin.Version, dragonId?.ToString(), dragonUrl, dragon, dressingRoomUrl, force);

                var loggedInUserId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();

                var user = ctx.Users.Find(loggedInUserId);
                foreach (var url in previewResult.Urls.Where(url => !skin.Previews.Any(x => x.Requestor == user && x.PreviewImage == url)))
                {
                    skin.Previews.Add(new Preview
                    {
                        DragonId = dragonId,
                        ScryerUrl = dragonId == null ? dragonUrl : null,
                        PreviewImage = url,
                        DragonData = dragon.ToString(),
                        PreviewTime = DateTime.UtcNow,
                        Requestor = ctx.Users.Find(loggedInUserId),
                        Version = skin.Version
                    });
                }

                await ctx.SaveChangesAsync();

                return View("PreviewResult", new PreviewModelPostViewModel
                {
                    SkinId = skinId,
                    Result = previewResult,
                    Dragon = dragon
                });
            }
        }

        [Route("upload", Name = "Upload")]
        public ActionResult Upload() => View(new UploadModelPost());

        [Route("upload", Name = "UploadPost")]
        [HttpPost]
        public async Task<ActionResult> Upload(UploadModelPost model)
        {
            var azureImageService = new AzureImageService();

            using (var ctx = new DataContext())
            {
                var randomizedId = GenerateId(5, ctx.Skins.Select(x => x.GeneratedId).ToList());
                var secretKey = GenerateId(7);
                Bitmap skinImage = null;
                try
                {
                    skinImage = (Bitmap)Image.FromStream(model.Skin.InputStream);
                    if (skinImage.Width != 350 || skinImage.Height != 350)
                    {
                        TempData["Error"] = "Image needs to be 350px x 350px. Just like FR.";
                        return View();
                    }

                }
                catch
                {
                    TempData["Error"] = "Upload is not a valid png image";
                    return View();
                }
                try
                {
                    model.Skin.InputStream.Position = 0;
                    var url = await azureImageService.WriteImage($@"skins\{randomizedId}.png", model.Skin.InputStream);

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
                        skin.Creator = ctx.Users.FirstOrDefault(x => x.Id == userId);
                        skin.Visibility = skin.Creator.DefaultVisibility;
                    }

                    ctx.Skins.Add(skin);
                    await ctx.SaveChangesAsync();
                    return View("UploadResult", new UploadModelPostViewModel
                    {
                        SkinId = randomizedId,
                        SecretKey = secretKey,
                        PreviewUrl = (await GenerateOrFetchPreview(randomizedId, skin.Version, "preview", string.Format(FRHelpers.DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null)).Urls[0],
                    });
                }
                catch
                {
                    TempData["Error"] = "Something went wrong uploading";
                    return View();
                }
            }
        }

        [Route("manage/skin/{skinId}/{secretKey}", Name = "Manage")]
        [Route("~/manage/skin/{skinId}/{secretKey}")] /* TODO: Delete this */
        public async Task<ActionResult> Manage(ManageModelGet model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.Include(x => x.Previews).FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found or secret invalid";
                    return RedirectToRoute("Home");
                }
                else
                {
                    if (skin.Creator != null)
                    {
                        if (!Request.IsAuthenticated)
                        {
                            TempData["Error"] = "This skin is linked to an acocunt, please log in to manage this skin.";
                            return RedirectToRoute("Home");
                        }
                        else if (skin.Creator.Id != HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>())
                        {
                            TempData["Error"] = "This skin is linked to a different account than the one that is logged in, you can only manage your own skins.";
                            return RedirectToRoute("Home");
                        }
                    }

                    if (skin.Coverage == null)
                        await UpdateCoverage(skin, ctx);
                    return View(new ManageModelViewModel
                    {
                        Skin = skin,
                        PreviewUrl = (await GenerateOrFetchPreview(model.SkinId, skin.Version, "preview", string.Format(FRHelpers.DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null)).Urls[0]
                    });
                }
            }
        }

        [Route("manage/skin", Name = "ManagePost")]
        [HttpPost]
        public async Task<ActionResult> Manage(ManageModelPost model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found or secret invalid";
                    return RedirectToRoute("Home");
                }
                else
                {
                    if (skin.Creator != null)
                    {
                        if (!Request.IsAuthenticated)
                        {
                            TempData["Error"] = "This skin is linked to an acocunt, please log in to manage this skin.";
                            return RedirectToRoute("Home");
                        }
                        else if (skin.Creator.Id != HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>())
                        {
                            TempData["Error"] = "This skin is linked to a different account than the one that is logged in, you can only manage your own or unclaimed skins.";
                            return RedirectToRoute("Home");
                        }
                    }

                    if (skin.GenderType != (int)model.Gender || skin.DragonType != (int)model.DragonType)
                    {
                        var azureImageService = new AzureImageService();
                        await azureImageService.DeleteImage($@"previews\{model.SkinId}\preview.png");
                    }

                    skin.Title = model.Title;
                    skin.Description = model.Description;
                    skin.GenderType = (int)model.Gender;
                    skin.DragonType = (int)model.DragonType;
                    skin.Visibility = model.Visibility;

                    await ctx.SaveChangesAsync();

                    TempData["Info"] = "Changes have been saved!";
                    return RedirectToRoute("Manage", new { model.SkinId, model.SecretKey });
                }
            }
        }

        [Route("manage/skin/update", Name = "UpdateSkinPost")]
        [HttpPost]
        public async Task<ActionResult> UpdateSkin(UpdateSkinPost model)
        {
            var azureImageService = new AzureImageService();

            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found or secret invalid";
                    return RedirectToRoute("Home");
                }
                else
                {
                    if (skin.Creator != null)
                    {
                        if (!Request.IsAuthenticated)
                        {
                            TempData["Error"] = "This skin is linked to an acocunt, please log in to update this skin.";
                            return RedirectToRoute("Home");
                        }
                        else if (skin.Creator.Id != HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>())
                        {
                            TempData["Error"] = "This skin is linked to a different account than the one that is logged in, you can only update your own or unclaimed skins.";
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
                        TempData["Error"] = "Image needs to be 350px x 350px. Just like FR.";
                        return RedirectToRoute("Manage", new { model.SkinId, model.SecretKey });
                    }

                }
                catch
                {
                    TempData["Error"] = "Upload is not a valid png image";
                    return RedirectToRoute("Manage", new { model.SkinId, model.SecretKey });
                }

                try
                {
                    model.Skin.InputStream.Position = 0;
                    var url = await azureImageService.WriteImage($@"skins\{model.SkinId}.png", model.Skin.InputStream);

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
                        skin.Creator = ctx.Users.FirstOrDefault(x => x.Id == userId);
                    }

                    await ctx.SaveChangesAsync();
                    TempData["Success"] = $"Skin succesfully updated to version <b>v{skin.Version}</b>!";
                    return RedirectToRoute("Manage", new { model.SkinId, model.SecretKey });
                }
                catch
                {
                    TempData["Error"] = "Something went wrong uploading";
                    return RedirectToRoute("Manage", new { model.SkinId, model.SecretKey });
                }
            }

        }

        [Route("delete", Name = "Delete")]
        public async Task<ActionResult> Delete(DeleteSkinPost model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found or secret invalid";
                    return RedirectToRoute("Home");
                }
                else
                {
                    var azureImageService = new AzureImageService();
                    await azureImageService.DeleteImage($@"skins\{model.SkinId}.png");
                    skin.Previews.Clear();
                    ctx.Skins.Remove(skin);
                    await ctx.SaveChangesAsync();
                }
            }
            return View();
        }

        static Dictionary<(int, int), byte[]> _dummyCache = new Dictionary<(int, int), byte[]>();

        public async Task<ActionResult> GetDummyDragon(int dragonType, int gender)
        {
            if (!_dummyCache.TryGetValue((dragonType, gender), out var bytes))
            {
                var dummyDragon = await FRHelpers.GetDragonBaseImage(string.Format(FRHelpers.DressingRoomDummyUrl, dragonType, gender));
                using (var memStream = new MemoryStream())
                {
                    dummyDragon.Save(memStream, ImageFormat.Png);
                    _dummyCache[(dragonType, gender)] = bytes = memStream.ToArray();
                }
            }

            return File(bytes, "image/png");
        }

        [Route("browse", Name = "Browse")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ActionResult> Browse(BrowseFilterModel filter)
        {
            var model = new BrowseViewModel { Filter = filter };
            using (var ctx = new DataContext())
            {
                var query = ctx.Skins
                    .Where(x => x.Visibility == SkinVisiblity.Visible)
                    .Where(x => filter.DragonTypes.Contains((DragonType)x.DragonType))
                    .Where(x => filter.Genders.Contains((Gender)x.GenderType))
                    .Where(x => filter.SkinTypes.Contains((BrowseFilterModel.SkinType)(x.Coverage >= 31 ? 1 : 0)));

                if (!string.IsNullOrEmpty(filter.Name))
                    query = query.Where(x => x.Title.Contains(filter.Name));

                model.TotalResults = query.Count();

                query = query.OrderByDescending(x => x.Id).Skip(filter.PageAmount * (filter.Page - 1)).Take(filter.PageAmount);

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
                    result.PreviewUrl = (await GenerateOrFetchPreview(result.SkinId, result.Version, "preview", string.Format(FRHelpers.DressingRoomDummyUrl, (int)result.DragonType, (int)result.Gender), null)).Urls[0];
            }

            return View(model);
        }

        [Route("link", Name = "LinkExisting")]
        public ActionResult LinkExistingSkin() => View();

        [HttpPost]
        [Route("link", Name = "LinkExistingPost")]
        public ActionResult LinkExistingSkin(ClaimSkinPostViewModel model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found or secret invalid";
                    return View();
                }
                int userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId<int>();

                if (skin.Creator != null)
                {
                    TempData["Error"] = "This skin is already linked to an acocunt, skins can only be claimed by a single account.";
                    return View();
                }

                skin.Creator = ctx.Users.Find(userId);
                ctx.SaveChanges();
            }
            TempData["Success"] = $"Succesfully linked skin '{model.SkinId}' to your account!";
            return RedirectToRoute("ManageAccount");
        }

        private bool IsAncientBreed(DragonType type) => type == DragonType.Gaoler;

        private async Task<PreviewResult> GenerateOrFetchPreview(string skinId, int version, string dragonId, string dragonUrl, DragonCache dragon, string dressingRoomUrl = null, bool force = false)
        {
            var result = new PreviewResult { Forced = force, RealDragon = dragonId != "preview" ? dragonId : null, DressingRoomUrl = dressingRoomUrl };

            var azureImageService = new AzureImageService();

            if (dragon == null)
                dragon = FRHelpers.ParseUrlForDragon(dragonUrl);

            Bitmap dragonImage = null;

            var azureImagePreviewPath = $@"previews\{skinId}\{(version == 1 ? "" : $@"{version}\")}{dragonId ?? dragon.ToString()}.png";
            if (force || !azureImageService.Exists(azureImagePreviewPath, out var previewUrl))
            {
                try
                {
                    dragonImage = await FRHelpers.GetDragonBaseImage(dragon);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Could not get dragon from Flight Rising:<br/>" + ex.Message;
                    result.Exception = ex.ToString();
                    return result;
                }

                Image skinImage;
                using (var skinImageStream = await azureImageService.GetImage($@"skins\{skinId}.png"))
                    skinImage = Image.FromStream(skinImageStream);

                var eyeMask = (Bitmap)Image.FromFile(Server.MapPath($@"\Masks\{(int)dragon.DragonType}_{(int)dragon.Gender}_{(dragon.EyeType == EyeType.Primal || dragon.EyeType == EyeType.MultiGaze ? (int)dragon.EyeType : 0)}_{(dragon.EyeType == EyeType.Primal ? (int)dragon.Element : 0)}.png"));

                if (dragon.EyeType == EyeType.MultiGaze)
                {
                    using (var graphics = Graphics.FromImage(eyeMask))
                    {
                        var normaleye = Image.FromFile(Server.MapPath($@"\Masks\{(int)dragon.DragonType}_{(int)dragon.Gender}_0_0.png"));
                        graphics.DrawImage(normaleye, new Rectangle(0, 0, 350, 350));
                        graphics.Save();
                    }
                }

                for (int x = 0; x < eyeMask.Width; x++)
                {
                    for (int y = 0; y < eyeMask.Height; y++)
                    {
                        var pixel = eyeMask.GetPixel(x, y);
                        if (pixel.A > 0)
                        {
                            var skinPixel = ((Bitmap)skinImage).GetPixel(x, y);
                            if (skinPixel.A > 0)
                                ((Bitmap)skinImage).SetPixel(x, y, System.Drawing.Color.FromArgb(255 - pixel.A, skinPixel.R, skinPixel.G, skinPixel.B));
                        }
                    }
                }

                using (var graphics = Graphics.FromImage(dragonImage))
                {
                    graphics.DrawImage(skinImage, new Rectangle(0, 0, 350, 350));
                }

                using (var memStream = new MemoryStream())
                {
                    dragonImage.Save(memStream, ImageFormat.Png);
                    memStream.Position = 0;

                    previewUrl = await azureImageService.WriteImage(azureImagePreviewPath, memStream);
                }
            }
            else
                result.Cached = true;

            result.Urls = new[] { previewUrl };

            async Task<string> GenerateApparelPreview(Bitmap invisibleDragon, string cacheUrl)
            {
                if (dragonImage == null)
                    dragonImage = (Bitmap)Image.FromStream(await azureImageService.GetImage(azureImagePreviewPath));

                using (var graphics = Graphics.FromImage(dragonImage))
                {
                    graphics.DrawImage(invisibleDragon, new Rectangle(0, 0, 350, 350));
                }

                using (var memStream = new MemoryStream())
                {
                    dragonImage.Save(memStream, ImageFormat.Png);
                    memStream.Position = 0;

                    return await azureImageService.WriteImage(cacheUrl, memStream);
                }

            }

            string apparelPreviewUrl = null;
            if (dressingRoomUrl != null)
            {
                var apparelIds = dragon.GetApparel();
                var cacheUrl = $@"previews\{skinId}\{dragonId ?? dragon.ToString()}_apparel.png";
                if (force || !azureImageService.Exists(cacheUrl, out apparelPreviewUrl))
                {
                    if (dragonId != null)
                        dressingRoomUrl = string.Format(FRHelpers.DressingRoomDragonApparalUrl, dragonId, string.Join(",", apparelIds.Concat(new[] { 22046 })));
                    else
                        dressingRoomUrl = string.Format(FRHelpers.DressingRoomDummyUrl, (int)dragon.DragonType, (int)dragon.Gender) + $"&apparel={string.Join(",", apparelIds.Concat(new[] { 22046 }))}";

                    var invisibleDragon = await FRHelpers.GetInvisibleDressingRoomDragon(dressingRoomUrl);
                    apparelPreviewUrl = await GenerateApparelPreview(invisibleDragon, cacheUrl);
                }
            }
            else if (dragonId != null && dragonId != "preview" && !IsAncientBreed(dragon.DragonType))
            {
                var cacheUrl = $@"previews\{skinId}\{dragonId}_apparel.png";
                if (force || !azureImageService.Exists(cacheUrl, out apparelPreviewUrl))
                {
                    var invisibleDragonWithApparel = await GetInvisibleDragonWithApparel(dragonId, azureImageService, force);

                    if (invisibleDragonWithApparel != null)
                        apparelPreviewUrl = await GenerateApparelPreview(invisibleDragonWithApparel, cacheUrl);

                }
            }
            if (apparelPreviewUrl != null)
                result.Urls = new[] { previewUrl, apparelPreviewUrl };

            return result;
        }

        private async Task<Bitmap> GetInvisibleDragonWithApparel(string dragonId, AzureImageService azureImageService, bool force = false)
        {
            Bitmap invisibleDwagon;
            var azureUrl = $@"dragoncache\{dragonId}_invisible.png";
            if (!force && azureImageService.Exists(azureUrl, out var cacheUrl))
            {
                using (var stream = await azureImageService.GetImage(azureUrl))
                    invisibleDwagon = (Bitmap)Image.FromStream(stream);
                return invisibleDwagon;
            }

            using (var client = new WebClient())
            {
                var dragonProfilePageTask = client.DownloadStringTaskAsync(string.Format(FRHelpers.DragonProfileUrl, dragonId));
                try
                {
                    var dragonProfileHtml = await dragonProfilePageTask;
                    var apparelFieldset = Regex.Matches(dragonProfileHtml, @"<fieldset([\s\S]*?)<\/fieldset>").Cast<Match>().Where(x => x.Success).ElementAt(1).Groups[1].Value;

                    var apparel = Regex.Matches(apparelFieldset, @"appPrev\((\d*)\)");
                    if (apparel.Count == 0)
                        return null;

                    var invisibleDragonBytesTask = client.DownloadDataTaskAsync(string.Format(FRHelpers.DressingRoomDragonApparalUrl, dragonId, string.Join(",", apparel.Cast<Match>().Where(x => x.Success).Select(x => x.Groups[1].Value).Concat(new[] { "22046" }))));

                    var invisibleDwagonImageBytes = await invisibleDragonBytesTask;

                    using (var memStream = new MemoryStream(invisibleDwagonImageBytes, false))
                        await azureImageService.WriteImage(azureUrl, memStream);

                    using (var memStream = new MemoryStream(invisibleDwagonImageBytes, false))
                    {
                        invisibleDwagon = (Bitmap)Image.FromStream(memStream);
                        return invisibleDwagon;
                    }
                }
                catch (Exception ex)
                {
                    // Something wrong fetching the image from FR
                    TempData["Error"] = "Could not get dragon from Flight Rising:<br/>" + ex.Message;
                    return null;
                }
            }
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

            var azureImageService = new AzureImageService();
            using (var stream = await azureImageService.GetImage($@"skins\{skin.GeneratedId}.png"))
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
    }
}