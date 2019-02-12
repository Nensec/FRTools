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
using System.Web;
using System.Web.Mvc;

namespace FRSkinTester.Controllers
{
    public class BaseController : Controller
    {
        protected const string DressingRoomDummyUrl = "http://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}";
        protected const string ScryerUrl = "http://flightrising.com/includes/scryer_getdragon.php?zord={0}";

        protected string ScrapeImageUrl(string scryerHtmlPage)
        {
            var imgTag = Regex.Match(scryerHtmlPage, @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
            return imgTag.Groups[1].Value;
        }


        protected Dragon ParseUrlForDragon(string dragonUrl)
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

        protected async Task<string> GenerateOrFetchPreview(string skinId, string dragonId, string dragonUrl, Dragon dragon)
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
    }
}