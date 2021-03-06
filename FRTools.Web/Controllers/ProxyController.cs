﻿using FRTools.Common;
using FRTools.Data;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FRTools.Web.Controllers
{
    [RoutePrefix("proxy")]
    public class ProxyController : BaseController
    {
        // In-memory caches to prevent requesting the same image over and over again, at least until site restart
        private static readonly Dictionary<(int, int), byte[]> _dummyCache = new Dictionary<(int, int), byte[]>();
        private static readonly Dictionary<(int, int, int), byte[]> _apparelCache = new Dictionary<(int, int, int), byte[]>();
        private static readonly Dictionary<(int, int, int), byte[]> _skinCache = new Dictionary<(int, int, int), byte[]>();
        private static readonly Dictionary<int, byte[]> _iconCache = new Dictionary<int, byte[]>();

        [Route("dummy/{dragonType}/{gender}", Name = "GetDummyDragon")]
        [Route("dummy", Name = "GetDummyDragonQueryString")]
        public async Task<ActionResult> GetDummyDragon(int dragonType, int gender, int? apparel = null, int? skin = null, int? gene = null)
        {
            if (apparel != null)
            {
                if (!_apparelCache.TryGetValue((dragonType, gender, apparel.Value), out var apparelBytes))
                {
                    using (var client = new WebClient())
                        _apparelCache[(dragonType, gender, apparel.Value)] = apparelBytes = await client.DownloadDataTaskAsync(string.Format(FRHelpers.DressingRoomDummyApparalUrl, dragonType, gender, apparel));
                }
                return File(apparelBytes, "image/png");
            }

            if (skin != null)
            {
                if (!_skinCache.TryGetValue((dragonType, gender, skin.Value), out var skinBytes))
                {
                    using (var client = new WebClient())
                        _skinCache[(dragonType, gender, skin.Value)] = skinBytes = await client.DownloadDataTaskAsync(string.Format(FRHelpers.DressingRoomDummySkinUrl, dragonType, gender, skin));
                }
                return File(skinBytes, "image/png");
            }

            if (gene != null)
            {
                // TODO: find a nice caching method for this, since it has random colors
                var item = DataContext.FRItems.FirstOrDefault(x => x.FRId == gene);
                int primary = 0, secondary = 0, tertiary = 0;
                int primaryColor = 0, secondaryColor = 0, tertiaryColor = 0;
                var itemSplit = item.Name.Split(':', '(');
                var random = new Random();
                byte[] geneBytes;
                switch (itemSplit[0])
                {
                    case "Primary Gene":
                        primary = (int)Enum.Parse(GeneratedFREnumExtentions.PrimaryGeneType((DragonType)dragonType), itemSplit[1]);
                        primaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
                        break;
                    case "Secondary Gene":
                        secondary = (int)Enum.Parse(GeneratedFREnumExtentions.SecondaryGeneType((DragonType)dragonType), itemSplit[1]);
                        secondaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
                        break;
                    case "Tertiary Gene":
                        tertiary = (int)Enum.Parse(GeneratedFREnumExtentions.TertiaryGeneType((DragonType)dragonType), itemSplit[1]);
                        tertiaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
                        break;
                }

                using (var client = new WebClient())
                    geneBytes = await client.DownloadDataTaskAsync(GeneratedFRHelpers.GenerateDragonImageUrl(dragonType, gender, 1, primary, primaryColor, secondary, secondaryColor, tertiary, tertiaryColor, random.Next(0, Enum.GetValues(typeof(Element)).Length + 1), random.Next(0, Enum.GetValues(typeof(EyeType)).Length + 1)));
                return File(geneBytes, "image/png");
            }

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

        [Route("icon/{id}/", Name = "GetIcon")]
        public async Task<ActionResult> GetIcon(int id)
        {
            if (!_iconCache.TryGetValue(id, out var bytes))
            {
                var item = DataContext.FRItems.FirstOrDefault(x => x.FRId == id);
                if (item == null)
                    return null;

                using (var client = new WebClient())
                    bytes = await client.DownloadDataTaskAsync("https://flightrising.com" + item.IconUrl);
            }

            return File(bytes, "image/png");
        }

    }
}