using FRSkinTester.Infrastructure;
using FRSkinTester.Infrastructure.DataModels;
using FRSkinTester.Models;
using System;
using System.Collections.Generic;
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
    public class BaseController : Controller
    {
        protected const string DressingRoomDummyUrl = "http://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}";
        protected const string ScryerUrl = "http://flightrising.com/includes/scryer_getdragon.php?zord={0}";
        protected const string DragonProfileUrl = "http://flightrising.com/main.php?dragon={0}";
        protected const string DressingRoomApparalUrl = "http://www1.flightrising.com/dgen/dressing-room/dragon?did={0}&apparel={1}";

        protected string ScrapeImageUrl(string scryerHtmlPage)
        {
            var imgTag = Regex.Match(scryerHtmlPage, @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
            return imgTag.Groups[1].Value;
        }

        protected DragonCache ParseUrlForDragon(string dragonUrl)
        {
            var dragon = new DragonCache();
            using (var ctx = new DataContext())
            {

                Match regexParse;
                if ((regexParse = Regex.Match(dragonUrl, @"gender=([\d]*)")).Success)
                    dragon.Gender = (Gender)int.Parse(regexParse.Groups[1].Value);
                if ((regexParse = Regex.Match(dragonUrl, @"breed=([\d]*)")).Success)
                    dragon.DragonType = (DragonType)int.Parse(regexParse.Groups[1].Value);
                if ((regexParse = Regex.Match(dragonUrl, @"auth=([a-z0-9]*)")).Success)
                    dragon.SHA1Hash = regexParse.Groups[1].Value;
                else
                    dragon.SHA1Hash = $"DUMMY_{(int)dragon.DragonType}_{(int)dragon.Gender}";

                var dbDragon = new DragonCache();

                if ((dbDragon = ctx.DragonCache.FirstOrDefault(x => x.SHA1Hash == dragon.SHA1Hash)) != null)
                    dragon = dbDragon;

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
                if ((regexParse = Regex.Match(dragonUrl, @"age=([\d]*)")).Success)
                    dragon.Age = (Age)int.Parse(regexParse.Groups[1].Value);
                if ((regexParse = Regex.Match(dragonUrl, @"apparel=([\d,]*)")).Success)
                    dragon.Apparel = regexParse.Groups[1].Value;

                ctx.DragonCache.Add(dragon);
                ctx.SaveChanges();
            }

            return dragon;
        }

        protected async Task<PreviewResult> GenerateOrFetchPreview(string skinId, string dragonId, string dragonUrl, DragonCache dragon, string dressingRoomUrl = null, bool force = false)
        {
            var result = new PreviewResult { Forced = force, RealDragon = dragonId != "preview" ? dragonId : null, DressingRoomUrl = dressingRoomUrl };

            var azureImageService = new AzureImageService();

            if (dragon == null)
                dragon = ParseUrlForDragon(dragonUrl);

            Bitmap dragonImage = null;

            if (force || !azureImageService.Exists($@"previews\{skinId}\{dragonId ?? dragon.ToString()}.png", out var previewUrl))
            {
                dragonImage = await GetDragonBaseImage(dragonUrl, dragon, azureImageService);

                Image skinImage;
                using (var skinImageStream = await azureImageService.GetImage($@"skins\{skinId}.png"))
                    skinImage = Image.FromStream(skinImageStream);

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

                using (var graphics = Graphics.FromImage(dragonImage))
                {
                    graphics.DrawImage(skinImage, new Rectangle(0, 0, 350, 350));
                }

                using (var memStream = new MemoryStream())
                {
                    dragonImage.Save(memStream, ImageFormat.Png);
                    memStream.Position = 0;

                    previewUrl = await azureImageService.WriteImage($@"previews\{skinId}\{dragonId ?? dragon.ToString()}.png", memStream);
                }
            }
            else
                result.Cached = true;

            result.Urls = new[] { previewUrl };

            async Task<string> GenerateApparelPreview(Bitmap invisibleDragon, string cacheUrl)
            {
                if (dragonImage == null)
                    dragonImage = (Bitmap)Image.FromStream(await azureImageService.GetImage($@"previews\{skinId}\{dragonId ?? dragon.ToString()}.png"));

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
                    var azureUrl = $@"dragoncache\{dragonId ?? dragon.ToString()}_invisible.png";
                    if (dragonId != null)
                        dressingRoomUrl = string.Format(DressingRoomApparalUrl, dragonId, string.Join(",", apparelIds.Concat(new[] { 22046 })));
                    else
                        dressingRoomUrl = string.Format(DressingRoomDummyUrl, (int)dragon.DragonType, (int)dragon.Gender) + $"&apparel={string.Join(",", apparelIds.Concat(new[] { 22046 }))}";

                    using (var client = new WebClient())
                    {
                        var invisibleDragonBytesTask = client.DownloadDataTaskAsync(dressingRoomUrl);

                        var invisibleDwagonImageBytes = await invisibleDragonBytesTask;
                        using (var memStream = new MemoryStream(invisibleDwagonImageBytes, false))
                            await azureImageService.WriteImage(azureUrl, memStream);

                        using (var memStream = new MemoryStream(invisibleDwagonImageBytes, false))
                        {
                            var invisibleDwagon = (Bitmap)Image.FromStream(memStream);
                            apparelPreviewUrl = await GenerateApparelPreview(invisibleDwagon, cacheUrl);
                        }
                    }
                }
            }
            else if (dragonId != null && dragonId != "preview")
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

        protected async Task<Bitmap> GetDragonBaseImage(string dragonUrl, DragonCache dragon, AzureImageService azureImageService)
        {
            if (dragon == null)
                dragon = ParseUrlForDragon(dragonUrl);

            Bitmap dwagonImage;
            string azureUrl = $@"dragoncache\{dragon.SHA1Hash}.png";

            if (azureImageService.Exists(azureUrl, out var cacheUrl))
            {
                using (var stream = await azureImageService.GetImage(azureUrl))
                    dwagonImage = (Bitmap)Image.FromStream(stream);
                return dwagonImage;
            }

            using (var client = new WebClient())
            {
                var dwagonImageBytesTask = client.DownloadDataTaskAsync(dragonUrl);
                try
                {
                    var dwagonImageBytes = await dwagonImageBytesTask;
                    using (var memStream = new MemoryStream(dwagonImageBytes, false))
                        await azureImageService.WriteImage(azureUrl, memStream);

                    using (var memStream = new MemoryStream(dwagonImageBytes, false))
                    {
                        dwagonImage = (Bitmap)Image.FromStream(memStream);
                        return dwagonImage;
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

        protected async Task<Bitmap> GetInvisibleDragonWithApparel(string dragonId, AzureImageService azureImageService, bool force = false)
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
                var dragonProfilePageTask = client.DownloadStringTaskAsync(string.Format(DragonProfileUrl, dragonId));
                try
                {
                    var dragonProfileHtml = await dragonProfilePageTask;
                    var apparelFieldset = Regex.Matches(dragonProfileHtml, @"<fieldset([\s\S]*?)<\/fieldset>").Cast<Match>().Where(x => x.Success).ElementAt(1).Groups[1].Value;

                    var apparel = Regex.Matches(apparelFieldset, @"appPrev\((\d*)\)");
                    if (apparel.Count == 0)
                        return null;

                    var invisibleDragonBytesTask = client.DownloadDataTaskAsync(string.Format(DressingRoomApparalUrl, dragonId, string.Join(",", apparel.Cast<Match>().Where(x => x.Success).Select(x => x.Groups[1].Value).Concat(new[] { "22046" }))));

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

        protected double? GetCoveragePercentage(Bitmap skinImage, Bitmap dragonImage)
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

        protected async Task UpdateCoverage(Skin skin, DataContext ctx)
        {
            Bitmap skinImage, dummyImage;

            var azureImageService = new AzureImageService();
            using (var stream = await azureImageService.GetImage($@"skins\{skin.GeneratedId}.png"))
                skinImage = (Bitmap)Image.FromStream(stream);

            dummyImage = await GetDragonBaseImage(string.Format(DressingRoomDummyUrl, skin.DragonType, skin.GenderType), null, azureImageService);

            skin.Coverage = GetCoveragePercentage(skinImage, dummyImage);
            skinImage.Dispose();
            dummyImage.Dispose();
            await ctx.SaveChangesAsync();
        }

        private Random _random = new Random(Guid.NewGuid().GetHashCode());

        protected string GenerateId(int length = 7, IEnumerable<string> mustNotMatch = null)
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

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}