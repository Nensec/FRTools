using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Web.Infrastructure;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.Web.Controllers
{
    public class BaseSkinController : BaseController
    {
        protected bool IsAncientBreed(DragonType type) => type == DragonType.Gaoler || type == DragonType.Banescale;

        protected async Task<PreviewResult> GenerateOrFetchPreview(string skinId, int version, string dragonId, string dragonUrl, DragonCache dragon, string dressingRoomUrl = null, bool force = false)
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

    }
}