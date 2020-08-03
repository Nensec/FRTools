using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.Common
{
    public static class SkinTester
    {
        public static async Task<PreviewResult> GenerateOrFetchPreview(string skinId, int version, string dragonId, string dragonUrl, DragonCache dragon, string dressingRoomUrl = null, bool force = false)
        {
            var result = new PreviewResult { Forced = force, RealDragon = dragonId != "preview" ? dragonId : null, DressingRoomUrl = dressingRoomUrl };

            if (dragon == null)
                dragon = FRHelpers.ParseUrlForDragon(dragonUrl);

            Bitmap dragonImage = null;

            var azureImagePreviewPath = $@"previews\{skinId}\{(version == 1 ? "" : $@"{version}\")}{dragonId ?? dragon.ToString()}.png";
            if (force || !new AzureImageService().Exists(azureImagePreviewPath, out var previewUrl))
            {
                try
                {
                    dragonImage = await FRHelpers.GetDragonBaseImage(dragon);
                }
                catch (Exception ex)
                {
                    result.Exception = ex.ToString();
                    return result;
                }

                Image skinImage;
                using (var skinImageStream = await new AzureImageService().GetImage($@"skins\{skinId}.png"))
                    skinImage = Image.FromStream(skinImageStream);

                var fixPixelFormat = FixPixelFormat((Bitmap)skinImage);
                if (fixPixelFormat != null)
                {
                    skinImage = fixPixelFormat;
                    using (var memStream = new MemoryStream())
                    {
                        skinImage.Save(memStream, ImageFormat.Png);
                        memStream.Position = 0;

                        _ = await new AzureImageService().WriteImage($@"skins\{skinId}.png", memStream);
                    }
                }

                var eyeMask = (Bitmap)Image.FromFile(CodeHelpers.MapPath($@"\Masks\{(int)dragon.DragonType}_{(int)dragon.Gender}_{(dragon.EyeType == EyeType.Primal || dragon.EyeType == EyeType.MultiGaze ? (int)dragon.EyeType : 0)}_{(dragon.EyeType == EyeType.Primal ? (int)dragon.Element : 0)}.png"));

                if (dragon.EyeType == EyeType.MultiGaze)
                {
                    using (var graphics = Graphics.FromImage(eyeMask))
                    {
                        var normaleye = Image.FromFile(CodeHelpers.MapPath($@"\Masks\{(int)dragon.DragonType}_{(int)dragon.Gender}_0_0.png"));
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

                using (var saveImageStream = new MemoryStream())
                {
                    dragonImage.Save(saveImageStream, ImageFormat.Png);
                    saveImageStream.Position = 0;

                    previewUrl = await new AzureImageService().WriteImage(azureImagePreviewPath, saveImageStream);
                }
            }
            else
                result.Cached = true;

            result.Urls = new[] { previewUrl };

            async Task<string> GenerateApparelPreview(Bitmap invisibleDragon, string cacheUrl)
            {
                if (dragonImage == null)
                    dragonImage = (Bitmap)Image.FromStream(await new AzureImageService().GetImage(azureImagePreviewPath));

                using (var graphics = Graphics.FromImage(dragonImage))
                {
                    graphics.DrawImage(invisibleDragon, new Rectangle(0, 0, 350, 350));
                }

                using (var saveApparelImageStream = new MemoryStream())
                {
                    dragonImage.Save(saveApparelImageStream, ImageFormat.Png);
                    saveApparelImageStream.Position = 0;

                    return await new AzureImageService().WriteImage(cacheUrl, saveApparelImageStream);
                }

            }

            string apparelPreviewUrl = null;
            if (dressingRoomUrl != null)
            {
                var apparelIds = dragon.GetApparel();
                var cacheUrl = $@"previews\{skinId}\{dragonId ?? dragon.ToString()}_apparel.png";
                if (force || !new AzureImageService().Exists(cacheUrl, out apparelPreviewUrl))
                {
                    if (dragonId != null)
                        dressingRoomUrl = string.Format(FRHelpers.DressingRoomDragonApparalUrl, dragonId, string.Join(",", apparelIds.Concat(new[] { 22046 })));
                    else
                        dressingRoomUrl = string.Format(FRHelpers.DressingRoomDummyUrl, (int)dragon.DragonType, (int)dragon.Gender) + $"&apparel={string.Join(",", apparelIds.Concat(new[] { 22046 }))}";

                    var invisibleDragon = await FRHelpers.GetInvisibleDressingRoomDragon(dressingRoomUrl);
                    apparelPreviewUrl = await GenerateApparelPreview(invisibleDragon, cacheUrl);
                }
            }
            else if (dragonId != null && dragonId != "preview" && !FRHelpers.IsAncientBreed(dragon.DragonType))
            {
                var cacheUrl = $@"previews\{skinId}\{dragonId}_apparel.png";
                if (force || !new AzureImageService().Exists(cacheUrl, out apparelPreviewUrl))
                {
                    var invisibleDragonWithApparel = await GetInvisibleDragonWithApparel(dragonId, force);

                    if (invisibleDragonWithApparel != null)
                        apparelPreviewUrl = await GenerateApparelPreview(invisibleDragonWithApparel, cacheUrl);

                }
            }
            if (apparelPreviewUrl != null)
                result.Urls = new[] { previewUrl, apparelPreviewUrl };

            return result;
        }

        public static async Task<Bitmap> GetInvisibleDragonWithApparel(string dragonId,  bool force = false)
        {
            Bitmap invisibleDwagon;
            var azureUrl = $@"dragoncache\{dragonId}_invisible.png";
            if (!force && new AzureImageService().Exists(azureUrl, out var cacheUrl))
            {
                using (var stream = await new AzureImageService().GetImage(azureUrl))
                    invisibleDwagon = (Bitmap)Image.FromStream(stream);
                return invisibleDwagon;
            }

            using (var client = new WebClient())
            {
                var dragonProfilePageTask = client.DownloadStringTaskAsync(string.Format(FRHelpers.DragonProfileUrl, dragonId));
                try
                {
                    var dragonProfileHtml = await dragonProfilePageTask;
                    var apparel = Regex.Matches(dragonProfileHtml, @"<div class="".*dragon-profile-apparel-item.*"".+data-itemid=""(\d+)"".+>.+</div>");

                    if (apparel.Count == 0)
                        return null;

                    var invisibleDragonBytesTask = client.DownloadDataTaskAsync(string.Format(FRHelpers.DressingRoomDragonApparalUrl, dragonId, string.Join(",", apparel.Cast<Match>().Where(x => x.Success).Select(x => x.Groups[1].Value).Concat(new[] { "22046" }))));

                    var invisibleDwagonImageBytes = await invisibleDragonBytesTask;

                    using (var memStream = new MemoryStream(invisibleDwagonImageBytes, false))
                        await new AzureImageService().WriteImage(azureUrl, memStream);

                    using (var memStream = new MemoryStream(invisibleDwagonImageBytes, false))
                    {
                        invisibleDwagon = (Bitmap)Image.FromStream(memStream);
                        return invisibleDwagon;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static Bitmap FixPixelFormat(Bitmap skinImage)
        {
            if (skinImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                var clone = new Bitmap(skinImage.Width, skinImage.Height, PixelFormat.Format32bppArgb);
                using (var gr = Graphics.FromImage(clone))
                    gr.DrawImage(skinImage, new Rectangle(0, 0, clone.Width, clone.Height));

                skinImage.Dispose();
                return clone;
            }
            return skinImage;
        }
    }
}
