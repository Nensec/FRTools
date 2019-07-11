﻿using FRTools.Data;
using FRTools.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Color = FRTools.Data.Color;

namespace FRTools.Web.Infrastructure
{
    public static class DragonHelpers
    {
        public const string DressingRoomDummyUrl = "http://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}";
        public const string ScryerUrl = "http://flightrising.com/includes/scryer_getdragon.php?zord={0}";
        public const string DragonProfileUrl = "http://flightrising.com/main.php?dragon={0}";
        public const string DressingRoomDragonApparalUrl = "http://www1.flightrising.com/dgen/dressing-room/dragon?did={0}&apparel={1}";
        public const string DressingRoomDummyApparalUrl = "http://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}&apparel={1}";

        public static async Task<Bitmap> GetInvisibleDressingRoomDragon(DragonCache dragon)
        {
            var azureImageService = new AzureImageService();
            var apparelIds = dragon.GetApparel();

            if (!apparelIds.Contains(22046))
                apparelIds = apparelIds.Concat(new[] { 22046 }).ToArray();

            var dressingRoomUrl = string.Format(DressingRoomDummyUrl, (int)dragon.DragonType, (int)dragon.Gender) + $"&apparel={string.Join(",", apparelIds)}";

            var azureUrl = $@"dragoncache\{dragon.ToString()}_invisible.png";
            using (var client = new WebClient())
            {
                var invisibleDragonBytesTask = client.DownloadDataTaskAsync(dressingRoomUrl);

                var invisibleDwagonImageBytes = await invisibleDragonBytesTask;
                using (var memStream = new MemoryStream(invisibleDwagonImageBytes, false))
                    await azureImageService.WriteImage(azureUrl, memStream);

                using (var memStream = new MemoryStream(invisibleDwagonImageBytes, false))
                {
                    var invisibleDwagon = (Bitmap)Image.FromStream(memStream);
                    return invisibleDwagon;
                }
            }
        }

        public static async Task<Bitmap> GetInvisibleDressingRoomDragon(string dressingRoomUrl)
        {
            if (dressingRoomUrl.Contains("dgen/dressing-room/dragon"))
            {
                var dragonId = int.Parse(Regex.Match(dressingRoomUrl, @"did=([\d]*)").Groups[1].Value);
                var dragon = GetDragonFromDragonId(dragonId);
                var apparelParse = ParseUrlForDragon(dressingRoomUrl);
                dragon.Apparel = apparelParse.Apparel;
                return await GetInvisibleDressingRoomDragon(dragon);
            }
            return await GetInvisibleDressingRoomDragon(ParseUrlForDragon(dressingRoomUrl));
        }

        public static DragonCache ParseUrlForDragon(string dragonUrl)
        {
            var dragon = new DragonCache();
            Match regexParse;
            if (dragonUrl.Contains("dgen/dressing-room/dragon"))
            {
                var dragonId = int.Parse(Regex.Match(dragonUrl, @"did=([\d]*)").Groups[1].Value);
                dragon = GetDragonFromDragonId(dragonId);
                if ((regexParse = Regex.Match(dragonUrl, @"apparel=([\d,]*)")).Success)
                    dragon.Apparel = regexParse.Groups[1].Value;
                return dragon;
            }

            using (var ctx = new DataContext())
            {
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
                    dragon.BodyGene = int.Parse(regexParse.Groups[1].Value);
                if ((regexParse = Regex.Match(dragonUrl, @"winggene=([\d]*)")).Success)
                    dragon.WingGene = int.Parse(regexParse.Groups[1].Value);
                if ((regexParse = Regex.Match(dragonUrl, @"tertgene=([\d]*)")).Success)
                    dragon.TertiaryGene = int.Parse(regexParse.Groups[1].Value);
                if ((regexParse = Regex.Match(dragonUrl, @"body=([\d]*)")).Success)
                    dragon.BodyColor = (Color)int.Parse(regexParse.Groups[1].Value);
                if ((regexParse = Regex.Match(dragonUrl, @"wings=([\d]*)")).Success)
                    dragon.WingColor = (Color)int.Parse(regexParse.Groups[1].Value);
                if ((regexParse = Regex.Match(dragonUrl, @"tert=([\d]*)")).Success)
                    dragon.TertiaryColor = (Color)int.Parse(regexParse.Groups[1].Value);
                if ((regexParse = Regex.Match(dragonUrl, @"age=([\d]*)")).Success)
                    dragon.Age = (Age)int.Parse(regexParse.Groups[1].Value);
                if ((regexParse = Regex.Match(dragonUrl, @"apparel=([\d,]*)")).Success)
                    dragon.Apparel = regexParse.Groups[1].Value;

                ctx.DragonCache.Add(dragon);
                ctx.SaveChanges();
            }

            return dragon;
        }

        private static string ScrapeImageUrl(string scryerHtmlPage)
        {
            var imgTag = Regex.Match(scryerHtmlPage, @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
            return imgTag.Groups[1].Value;
        }

        public static string GetDragonImageUrlFromDragonId(int dragonId)
        {
            using (var client = new WebClient())
            {
                var htmlPage = client.DownloadString(string.Format(ScryerUrl, dragonId));
                return ScrapeImageUrl(htmlPage);
            }
        }

        public static DragonCache GetDragonFromDragonId(int dragonId) => ParseUrlForDragon(GetDragonImageUrlFromDragonId(dragonId));

        public static Task<Bitmap> GetDragonBaseImage(string dragonUrl) => GetDragonBaseImage(ParseUrlForDragon(dragonUrl));

        public static async Task<Bitmap> GetDragonBaseImage(DragonCache dragon)
        {
            Bitmap dwagonImage;
            string azureUrl = $@"dragoncache\{dragon.SHA1Hash}.png";
            var azureImageService = new AzureImageService();

            if (azureImageService.Exists(azureUrl, out _))
            {
                using (var stream = await azureImageService.GetImage(azureUrl))
                    dwagonImage = (Bitmap)Image.FromStream(stream);
                return dwagonImage;
            }

            using (var client = new WebClient())
            {
                var dwagonImageBytesTask = client.DownloadDataTaskAsync(dragon.ConstructImageString());
                var dwagonImageBytes = await dwagonImageBytesTask;
                using (var memStream = new MemoryStream(dwagonImageBytes, false))
                    await azureImageService.WriteImage(azureUrl, memStream);

                using (var memStream = new MemoryStream(dwagonImageBytes, false))
                {
                    dwagonImage = (Bitmap)Image.FromStream(memStream);
                    return dwagonImage;
                }
            }
        }
    }
}