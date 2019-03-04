using FRSkinTester.Infrastructure;
using FRSkinTester.Infrastructure.DataModels;
using FRSkinTester.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FRSkinTester.Controllers
{
    public class SkinController : BaseController
    {
        [Route("Preview/{skinId}", Name = "Preview")]
        [Route("Home/Preview")] // TODO: delete dis
        public async Task<ActionResult> Preview(PreviewModelGet model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToAction("Index");
                }
                if (skin.Coverage == null)
                    await UpdateCoverage(skin, ctx);
                return View(new PreviewModelPost
                {
                    Title = skin.Title,
                    Description = skin.Description,
                    SkinId = model.SkinId,
                    PreviewUrl = await GenerateOrFetchPreview(model.SkinId, "preview", string.Format(DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null),
                    Coverage = skin.Coverage,
                    Creator = skin.Creator
                });
            }
        }

        [HttpPost]
        [Route("Preview/{skinId}", Name = "PreviewPost")]
        public async Task<ActionResult> Preview(PreviewModelPost model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Preview", new { model.SkinId });

            string dwagonUrl = null;
            using (var client = new WebClient())
            {
                var htmlPage = await client.DownloadStringTaskAsync(new Uri(string.Format(ScryerUrl, model.DragonId)));
                dwagonUrl = ScrapeImageUrl(htmlPage);
            }

            if (dwagonUrl.StartsWith(".."))
            {
                TempData["Error"] = $"<b>{model.DragonId}</b> appears to be an invalid dragon id";
                return RedirectToAction("Preview", new { model.SkinId });
            }

            var dragon = ParseUrlForDragon(dwagonUrl);

            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToAction("Index");
                }

                if (skin.DragonType != (int)dragon.DragonType)
                {
                    TempData["Error"] = $"This skin is meant for a <b>{(DragonType)skin.DragonType} {(Gender)skin.GenderType}</b>, the dragon you provided is a <b>{dragon.DragonType} {dragon.Gender}</b>";
                    return RedirectToAction("Preview", new { model.SkinId });
                }

                if (skin.GenderType != (int)dragon.Gender)
                {
                    TempData["Error"] = $"This skin is meant for a <b>{(Gender)skin.GenderType}</b>, the dragon you provided is a <b>{dragon.Gender}</b>";
                    return RedirectToAction("Preview", new { model.SkinId });
                }

                var url = await GenerateOrFetchPreview(model.SkinId, model.DragonId.ToString(), dwagonUrl, dragon);
                if (url == null)
                    return RedirectToAction("Preview", new { model.SkinId });

                skin.Previews.Add(new Preview
                {
                    DragonId = model.DragonId,
                    PreviewImage = url,
                    DragonData = dragon.ToString()
                });

                await ctx.SaveChangesAsync();

                return View("PreviewResult", new PreviewModelPostViewModel
                {
                    SkinId = model.SkinId,
                    ImageResultUrl = url,
                    Dragon = dragon
                });
            }
        }

        [Route("Preview/{skinId}/Scry", Name = "PreviewScryer")]
        [Route("Home/PreviewScryer")] // TODO: delete dis
        public async Task<ActionResult> PreviewScryer(PreviewModelGet model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToAction("Index");
                }
                if (skin.Coverage == null)
                    await UpdateCoverage(skin, ctx);

                return View(new PreviewScryerModelPost
                {
                    Title = skin.Title,
                    Description = skin.Description,
                    SkinId = model.SkinId,
                    PreviewUrl = await GenerateOrFetchPreview(model.SkinId, "preview", string.Format(DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null),
                    Coverage = skin.Coverage,
                    Creator = skin.Creator
                });
            }
        }

        [HttpPost]
        [Route("Preview/{skinId}/Scry", Name = "PreviewScryerPost")]
        public async Task<ActionResult> PreviewScryer(PreviewScryerModelPost model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("PreviewScryer", new { model.SkinId });

            var dragon = ParseUrlForDragon(model.ScryerUrl);

            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found";
                    return RedirectToAction("Index");
                }

                if (skin.DragonType != (int)dragon.DragonType)
                {
                    TempData["Error"] = $"This skin is meant for a <b>{(DragonType)skin.DragonType} {(Gender)skin.GenderType}</b>, the dragon you provided is a <b>{dragon.DragonType} {dragon.Gender}</b>";
                    return RedirectToAction("PreviewScryer", new { model.SkinId });
                }

                if (skin.GenderType != (int)dragon.Gender)
                {
                    TempData["Error"] = $"This skin is meant for a <b>{(Gender)skin.GenderType}</b>, the dragon you provided is a <b>{dragon.Gender}</b>";
                    return RedirectToAction("PreviewScryer", new { model.SkinId });
                }


                var url = await GenerateOrFetchPreview(model.SkinId, null, model.ScryerUrl, dragon);
                if (url == null)
                    return RedirectToAction("PreviewScryer", new { model.SkinId });

                skin.Previews.Add(new Preview
                {
                    ScryerUrl = model.ScryerUrl,
                    PreviewImage = url,
                    DragonData = dragon.ToString()
                });

                await ctx.SaveChangesAsync();

                return View("PreviewResult", new PreviewModelPostViewModel
                {
                    SkinId = model.SkinId,
                    ImageResultUrl = url,
                    Dragon = dragon
                });
            }
        }


        [Route("Upload", Name = "Upload")]
        public ActionResult Upload() => View(new UploadModelPost());

        [Route("Upload", Name = "UploadPost")]
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
                        var dwagonImageBytes = client.DownloadDataTaskAsync(string.Format(DressingRoomDummyUrl, (int)model.DragonType, (int)model.Gender));
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
                    }

                    ctx.Skins.Add(skin);
                    await ctx.SaveChangesAsync();
                    return View("UploadResult", new UploadModelPostViewModel
                    {
                        SkinId = randomizedId,
                        SecretKey = secretKey,
                        PreviewUrl = await GenerateOrFetchPreview(randomizedId, "preview", string.Format(DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null),
                    });
                }
                catch
                {
                    TempData["Error"] = "Something went wrong uploading";
                    return View();
                }
            }
        }

        [Route("Manage/Skin/{skinId}/{secretKey}", Name = "Manage")]
        [Route("Manage/{skinId}/{secretKey}")] // TODO: delete dis
        [Route("Home/Manage")] // TODO: delete dis
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
                        PreviewUrl = await GenerateOrFetchPreview(model.SkinId, "preview", string.Format(DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null)
                    });
                }
            }
        }

        [Route("Manage/Skin", Name = "ManagePost")]
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

                    await ctx.SaveChangesAsync();

                    TempData["Info"] = "Changes have been saved!";
                    return RedirectToAction("Manage", new { model.SkinId, model.SecretKey });
                }
            }
        }

        [Route("Delete", Name = "Delete")]
        [Route("Home/Delete")] // TODO: delete dis
        public async Task<ActionResult> Delete(DeleteSkinPost model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found or secret invalid";
                    return RedirectToAction("Index");
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
                var dummyDragon = await GetDragonBaseImage(string.Format(DressingRoomDummyUrl, dragonType, gender), null, new AzureImageService());
                using (var memStream = new MemoryStream())
                {
                    dummyDragon.Save(memStream, ImageFormat.Png);
                    _dummyCache[(dragonType, gender)] = bytes = memStream.ToArray();
                }
            }

            return File(bytes, "image/png");
        }
    }
}