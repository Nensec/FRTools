using AthenaV3.Business.Services.Image;
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
                var randomizedId = GenerateId(5, null, ctx.Skins.Select(x => x.GeneratedId).ToList());
                var secretKey = GenerateId(7, randomizedId.Select(x => (int)x).Sum());
                var url = await azureImageService.WriteImage($@"skins\{randomizedId}.png", model.Skin.InputStream);
                var skin = new Skin
                {
                    GeneratedId = randomizedId,
                    SecretKey = secretKey,
                    Title = model.Title,
                    Description = model.Description,
                    DragonType = (int)model.DragonType,
                    GenderType = (int)model.Gender
                };

                ctx.Skins.Add(skin);
                await ctx.SaveChangesAsync();
                return View("UploadResult", new UploadModelPostResult
                {
                    SkinId = randomizedId,
                    SecretKey = secretKey
                });
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

                return View(new PreviewModelPost
                {
                    Title = skin.Title,
                    Description = skin.Description,
                    SkinId = model.SkinId,
                    PreviewUrl = await GenerateOrFetchPreview(model.SkinId, "preview", string.Format(DressingRoomDummyUrl, skin.DragonType, skin.GenderType))
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Preview(PreviewModelPost model)
        {
            if (!ModelState.IsValid)
                return View();

            string dwagonUrl = null;
            using (var client = new WebClient())
            {
                var htmlPage = await client.DownloadStringTaskAsync(new Uri(string.Format(ScryerUrl, model.DragonId)));
                dwagonUrl = ScrapeImageUrl(htmlPage);
            }

            if (dwagonUrl.StartsWith(".."))
            {
                TempData["Error"] = $"<b>{model.DragonId}</b> appears to be an invalid dragon id";
                return View();
            }

            var url = await GenerateOrFetchPreview(model.SkinId, model.DragonId.ToString(), dwagonUrl);
            if(url == null)
                return View();

            return View("PreviewResult", new PreviewModelPostResult
            {
                SkinId = model.SkinId,
                ImageResultUrl = url
            });
        }

        private string ScrapeImageUrl(string scryerHtmlPage)
        {
            var imgTag = Regex.Match(scryerHtmlPage, @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
            return imgTag.Groups[1].Value;
        }

        private async Task<string> GenerateOrFetchPreview(string skinId, string dragonId, string dragonUrl)
        {
            var azureImageService = new AzureImageService();

            if (!await azureImageService.Exists($@"previews\{skinId}\{dragonId}.png", out var url))
            {
                using (var client = new WebClient())
                {
                    var dwagonImageBytes = client.DownloadDataTaskAsync(dragonUrl);
                    Image dwagonImage = null;
                    try
                    {
                        using (var memStream = new MemoryStream(await dwagonImageBytes, false))
                            dwagonImage = Image.FromStream(memStream);
                    }
                    catch (Exception ex)
                    {
                        // Something wrong fetching the image from FR
                        TempData["Error"] = "Could not get dragon from Flight Rising:<br/>" + ex.Message;
                        return null;
                    }

                    var skinImageStream = azureImageService.GetImage($@"skins\{skinId}.png");
                    var skinImage = Image.FromStream(await skinImageStream);

                    using (var graphics = Graphics.FromImage(dwagonImage))
                    {
                        graphics.DrawImage(skinImage, new Rectangle(0, 0, 350, 350));
                    }

                    using (var memStream = new MemoryStream())
                    {
                        dwagonImage.Save(memStream, ImageFormat.Png);
                        memStream.Position = 0;

                        url = await azureImageService.WriteImage($@"previews\{skinId}\{dragonId}.png", memStream);
                    }
                }
            }

            return url;
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
                }
            }
            return View();
        }

        private string GenerateId(int length = 7, int? seed = null, IEnumerable<string> mustNotMatch = null)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string id = "";
            var rand = seed != null ? new Random(seed.Value) : new Random();
            for (int i = 0; i < length; i++)
                id += chars.Skip(rand.Next(chars.Length)).First();
            if (mustNotMatch?.Contains(id) == true)
            {
                return GenerateId(length, seed, mustNotMatch);
            }
            return id;
        }
    }
}