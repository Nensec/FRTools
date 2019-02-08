using AthenaV3.Business.Services.Image;
using FRSkinTester.Infrastructure;
using FRSkinTester.Infrastructure.DataModels;
using FRSkinTester.Models;
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
using System.Web.Mvc;

namespace FRSkinTester.Controllers
{
    public class HomeController : Controller
    {
        private const string DressingRoomDummyUrl = "http://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}";
        private const string ScryerUrl = "http://flightrising.com/includes/scryer_getdragon.php?zord={0}";


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View(new UploadModelPost());
        }

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
                    ctx.Skins.Add(skin);
                    await ctx.SaveChangesAsync();
                    return View("UploadResult", new UploadModelPostViewModel
                    {
                        SkinId = randomizedId,
                        SecretKey = secretKey
                    });
                }
                catch
                {
                    TempData["Error"] = "Something went wrong uploading";
                    return View();
                }
            }
        }

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
                    Coverage = skin.Coverage
                });
            }
        }

        [HttpPost]
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
                    Coverage = skin.Coverage
                });
            }
        }

        [HttpPost]
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

        private string ScrapeImageUrl(string scryerHtmlPage)
        {
            var imgTag = Regex.Match(scryerHtmlPage, @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
            return imgTag.Groups[1].Value;
        }

        private Dragon ParseUrlForDragon(string dragonUrl)
        {
            var dragon = new Dragon();

            Match regexParse;
            if ((regexParse = Regex.Match(dragonUrl, @"gender=([\d]*)")).Success)
                dragon.Gender = (Gender)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"breed=([\d]*)")).Success)
                dragon.DragonType = (DragonType)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"element=([\d]*)")).Success)
                dragon.Element = (Element)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"eyetype=([\d]*)")).Success)
                dragon.EyeType = (EyeType)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"bodygene=([\d]*)")).Success)
                dragon.BodyGene = (BodyGene)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"winggene=([\d]*)")).Success)
                dragon.WingGene = (WingGene)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"tertgene=([\d]*)")).Success)
                dragon.TertiaryGene = (TertiaryGene)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"body=([\d]*)")).Success)
                dragon.BodyColor = (Models.Color)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"wings=([\d]*)")).Success)
                dragon.WingColor = (Models.Color)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"tert=([\d]*)")).Success)
                dragon.TertiaryColor = (Models.Color)int.Parse(regexParse.Groups[1].Value);

            return dragon;
        }

        private async Task<string> GenerateOrFetchPreview(string skinId, string dragonId, string dragonUrl, Dragon dragon)
        {
            var azureImageService = new AzureImageService();

            if (dragon == null)
                dragon = ParseUrlForDragon(dragonUrl);

            if (!await azureImageService.Exists($@"previews\{skinId}\{dragonId ?? dragon.ToString()}.png", out var url))
            {
                using (var client = new WebClient())
                {
                    var dwagonImageBytes = client.DownloadDataTaskAsync(dragonUrl);
                    Bitmap dwagonImage = null;
                    try
                    {
                        using (var memStream = new MemoryStream(await dwagonImageBytes, false))
                            dwagonImage = (Bitmap)Image.FromStream(memStream);
                    }
                    catch (Exception ex)
                    {
                        // Something wrong fetching the image from FR
                        TempData["Error"] = "Could not get dragon from Flight Rising:<br/>" + ex.Message;
                        return null;
                    }

                    var skinImageStream = azureImageService.GetImage($@"skins\{skinId}.png");
                    var skinImage = Image.FromStream(await skinImageStream);
                    var eyeMask = (Bitmap)Image.FromFile(Server.MapPath($@"\Masks\{(int)dragon.DragonType}_{(int)dragon.Gender}_{(dragon.EyeType == EyeType.Primal || dragon.EyeType == EyeType.MultiGaze ? (int)dragon.EyeType : 0)}_{(dragon.EyeType == EyeType.Primal ? (int)dragon.Element : 0)}.png"));

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

                    using (var graphics = Graphics.FromImage(dwagonImage))
                    {
                        graphics.DrawImage(skinImage, new Rectangle(0, 0, 350, 350));
                    }

                    using (var memStream = new MemoryStream())
                    {
                        dwagonImage.Save(memStream, ImageFormat.Png);
                        memStream.Position = 0;

                        url = await azureImageService.WriteImage($@"previews\{skinId}\{dragonId ?? dragon.ToString()}.png", memStream);
                    }
                }
            }

            return url;
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
            var azureImageService = new AzureImageService();
            var skinImage = (Bitmap)Image.FromStream(await azureImageService.GetImage($@"skins\{skin.GeneratedId}.png"));
            Bitmap dummyImage = null;

            using (var client = new WebClient())
            {
                var dwagonImageBytes = client.DownloadDataTaskAsync(string.Format(DressingRoomDummyUrl, skin.DragonType, skin.GenderType));
                try
                {
                    using (var memStream = new MemoryStream(await dwagonImageBytes, false))
                        dummyImage = (Bitmap)Image.FromStream(memStream);
                }
                catch
                {
                    return;
                }
            }

            skin.Coverage = GetCoveragePercentage(skinImage, dummyImage);
            skinImage.Dispose();
            dummyImage.Dispose();
            await ctx.SaveChangesAsync();
        }

        public async Task<ActionResult> Manage(ManageModelGet model)
        {
            using (var ctx = new DataContext())
            {
                var skin = ctx.Skins.Include(x => x.Previews).FirstOrDefault(x => x.GeneratedId == model.SkinId && x.SecretKey == model.SecretKey);
                if (skin == null)
                {
                    TempData["Error"] = "Skin not found or secret invalid";
                    return RedirectToAction("Index");
                }
                else
                {
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

        [HttpPost]
        public async Task<ActionResult> Manage(ManageModelPost model)
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

        private Random _random = new Random(Guid.NewGuid().GetHashCode());
        private string GenerateId(int length = 7, IEnumerable<string> mustNotMatch = null)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string id = "";
            for (int i = 0; i < length; i++)
                id += chars.Skip(_random.Next(chars.Length)).First();
            if (mustNotMatch?.Contains(id) == true)
            {
                return GenerateId(length, mustNotMatch);
            }
            return id;
        }
    }
}