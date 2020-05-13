using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Color = FRTools.Data.Color;

namespace FRTools.Common
{
    public static class FRHelpers
    {
        //                                          https://www1.flightrising.com/dgen/dressing-room/dummy?breed=18&gender=0.png
        public const string DressingRoomDummyUrl = "https://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}";
        public const string ScryerUrl = "https://flightrising.com/includes/scryer_getdragon.php?zord={0}";
        public const string DragonProfileUrl = "https://flightrising.com/main.php?dragon={0}";
        public const string DressingRoomDragonApparalUrl = "https://www1.flightrising.com/dgen/dressing-room/dragon?did={0}&apparel={1}";
        public const string DressingRoomDummyApparalUrl = "https://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}&apparel={1}";

        public static bool IsAncientBreed(DragonType type) => type == DragonType.Gaoler || type == DragonType.Banescale;


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

        public static string GetDragonImageUrlFromScryId(int scryId)
        {
            var scryUrl = $"https://www1.flightrising.com/scrying/predict?morph={scryId}";
            using (var client = new WebClient())
            {
                var htmlPage = client.DownloadString(scryUrl);
                var scryPageUrlParse = Regex.Match(htmlPage, @"breed=(\d+)&gender=(\d+)&age=(\d+)&bodygene=(\d+)&body=(\d+)&winggene=(\d+)&wings=(\d+)&tertgene=(\d+)&tert=(\d+)&element=(\d+)&eyetype=(\d+)");
                var result = client.UploadValues("https://www1.flightrising.com/scrying/ajax-predict", new System.Collections.Specialized.NameValueCollection
                {
                    { "breed", scryPageUrlParse.Groups[1].Value },
                    { "gender", scryPageUrlParse.Groups[2].Value },
                    { "age", scryPageUrlParse.Groups[3].Value },
                    { "bodygene", scryPageUrlParse.Groups[4].Value },
                    { "body", scryPageUrlParse.Groups[5].Value },
                    { "winggene", scryPageUrlParse.Groups[6].Value },
                    { "wings", scryPageUrlParse.Groups[7].Value },
                    { "tertgene", scryPageUrlParse.Groups[8].Value },
                    { "tert", scryPageUrlParse.Groups[9].Value },
                    { "element", scryPageUrlParse.Groups[10].Value },
                    { "eyetype", scryPageUrlParse.Groups[11].Value },
                });
                var str = Encoding.UTF8.GetString(result);
                var dragonUrl = JsonConvert.DeserializeObject<dynamic>(str).dragon_url;
                return "https://www1.flightrising.com" + dragonUrl;
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
                var dwagonImageBytesTask = client.DownloadDataTaskAsync(dragon.ConstructImageString() ?? string.Format(DressingRoomDummyUrl, (int)dragon.DragonType, (int)dragon.Gender));
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

        public static Task<FRUser> GetOrUpdateFRUser(string username, DataContext ctx = null) => GetOrUpdateFRUser(username, null, ctx);
        public static Task<FRUser> GetOrUpdateFRUser(int userId, DataContext ctx = null) => GetOrUpdateFRUser(null, userId, ctx);

        private static Task<FRUser> GetOrUpdateFRUser(string username, int? userId, DataContext ctx = null)
        {
            async Task<FRUser> getFRUser()
            {
                var dispose = false;
                if (dispose = ctx == null)
                    ctx = new DataContext();

                var frUser = ctx.FRUsers.FirstOrDefault(x => x.Username == username || x.FRId == userId);

                if (frUser == null)
                {
                    var (frName, frId) = GetFRUserInfo(username, userId);

                    if (frName == null)
                        return null;

                    frUser = ctx.FRUsers.FirstOrDefault(x => x.Username == frName || x.FRId == frId) ?? ctx.FRUsers.Add(new FRUser());
                    frUser.Username = frName;
                    frUser.FRId = frId;
                    frUser.LastUpdated = DateTime.UtcNow;

                    ctx.SaveChanges();
                    if (dispose)
                        ctx.Dispose();
                }
                else
                {
                    frUser = await frUser.UpdateFRUser();
                    await ctx.SaveChangesAsync();
                }

                if (dispose)
                    ctx.Dispose();

                return frUser;
            }
            return getFRUser();
        }

        public static async Task<FRUser> UpdateFRUser(this FRUser frUser)
        {
            // Only update if it hasn't been a day to avoid spamming FR server
            if (DateTime.UtcNow < frUser.LastUpdated.AddDays(1))
                return frUser;

            var (frName, frId) = GetFRUserInfo(null, frUser.FRId);

            if (frName == null)
                return null;

            frUser.Username = frName;
            frUser.FRId = frId;
            frUser.LastUpdated = DateTime.UtcNow;
            await Task.Delay(50);
            return frUser;
        }

        private static (string Username, int UserId) GetFRUserInfo(string username, int? userId)
        {
            string url = $"https://www1.flightrising.com/clan-profile/{(userId?.ToString() ?? $"n/{username}")}";
            using (var client = new WebClient())
            {
                var userProfilePage = client.DownloadString(url);
                if (userProfilePage.Contains("404 - Page Not Found") || userProfilePage.Contains("404: User not found"))
                    return default;

                var userBio = Regex.Match(userProfilePage, @"<div class=""userdata-section"" style=""height:136px;"">[\s\S]+?<span style=""position:absolute; top:8px; left:8px; color:#731d08; font-weight:bold; font-size:16px;"">\s+([\s\S]+?)\s+</span>[\s\S]+?<span>([\d]+?)</span>[\s\S]+?</div>");
                var frName = userBio.Groups[1].Value;
                var frId = int.Parse(userBio.Groups[2].Value);

                return (frName, frId);
            }
        }
    }
}