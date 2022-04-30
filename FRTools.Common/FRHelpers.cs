using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Color = FRTools.Data.Color;

namespace FRTools.Common
{
    public static class FRHelpers
    {
        public const string DressingRoomDummyUrl = "https://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}";
        public const string ScryerUrl = "https://flightrising.com/includes/scryer_getdragon.php?zord={0}";
        public const string DragonProfileUrl = "https://www1.flightrising.com/dragon/{0}";
        public const string DragonProfileUrlNoScenic = "https://www1.flightrising.com/dragon/{0}?view=default";
        public const string DressingRoomDragonApparalUrl = "https://www1.flightrising.com/dgen/dressing-room/dragon?did={0}&apparel={1}";
        public const string DressingRoomDummyApparalUrl = "https://www1.flightrising.com/dgen/dressing-room/dummy?breed={0}&gender={1}&apparel={2}";
        public const string DressingRoomDummySkinUrl = "https://www1.flightrising.com/hoard/preview-image?gender={1}&breed={0}&item={2}";
        public const string ItemFetchUrl = "https://flightrising.com/includes/itemajax.php?id={0}&tab={1}";
        public const string FamiliarArtUrl = "https://www1.flightrising.com/static/cms/familiar/art/{0}.png";
        public const string SceneArtUrl = "https://www1.flightrising.com/static/cms/scene/{0}.png";
        public const string UserProfileUrl = "https://www1.flightrising.com/clan-profile/{0}";
        public const string GameDatabaseUrl = "https://www1.flightrising.com/game-database/item/{0}";
        public const string MarketplaceUrl = "https://www1.flightrising.com/market/treasure/";
        public const string MarketplaceFetchUrl = "https://www1.flightrising.com/market/ajax/get-items?mode=treasure&tab={0}";

        public static string GetRenderUrl(int dragonId) => $"https://www1.flightrising.com/rendern/350/{(Math.Floor(dragonId / 100d) + 1)}/{dragonId}_350.png";

        private static Dictionary<string, DragonCache> Cache { get; } = new Dictionary<string, DragonCache>(StringComparer.InvariantCultureIgnoreCase);

        public static async Task<Bitmap> GetInvisibleDressingRoomDragon(DragonCache dragon)
        {
            var azureImageService = new AzureFileService();
            var apparelIds = dragon.GetApparel();

            if (!apparelIds.Contains(22046))
                apparelIds = apparelIds.Concat(new[] { 22046 }).ToArray();

            var dressingRoomUrl = string.Format(DressingRoomDummyUrl, (int)dragon.DragonType, (int)dragon.Gender) + $"&apparel={string.Join(",", apparelIds)}";

            var azureUrl = $@"dragoncache\{dragon}_invisible.png";
            using (var client = new WebClient())
            {
                var invisibleDragonBytesTask = client.DownloadDataTaskAsync(dressingRoomUrl);

                var invisibleDwagonImageBytes = await invisibleDragonBytesTask;
                using (var memStream = new MemoryStream(invisibleDwagonImageBytes, false))
                    await azureImageService.WriteFile(azureUrl, memStream);

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

        public static DragonCache ParseUrlForDragon(string dragonUrl, string skinId = null, int? version = null, bool forced = false)
        {
            var dragon = new DragonCache();
            Match regexParse;
            if (dragonUrl.Contains("dgen/dressing-room/dragon"))
            {
                var dragonId = int.Parse(Regex.Match(dragonUrl, @"did=([\d]*)").Groups[1].Value);
                dragon = GetDragonFromDragonId(dragonId);
                dragon.FRDragonId = dragonId;
                if ((regexParse = Regex.Match(dragonUrl, @"apparel=([\d,]*)")).Success)
                    dragon.Apparel = regexParse.Groups[1].Value;
                return dragon;
            }

            if ((regexParse = Regex.Match(dragonUrl, @"gender=([\d]*)")).Success)
                dragon.Gender = (Gender)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"breed=([\d]*)")).Success)
                dragon.DragonType = (DragonType)int.Parse(regexParse.Groups[1].Value);
            if ((regexParse = Regex.Match(dragonUrl, @"auth=([a-z0-9]*)")).Success)
                dragon.SHA1Hash = regexParse.Groups[1].Value;
            else
            {
                if (skinId != null && version != null)
                    dragon.SHA1Hash = $"DUMMY_{(int)dragon.DragonType}_{(int)dragon.Gender}_{skinId}_v{version}";
                else
                {
                    dragon.SHA1Hash = $"DUMMY_{(int)dragon.DragonType}_{(int)dragon.Gender}";
                    Debug.WriteLine("Caching dummy image without skin info");
                }
            }

            if (forced || !Cache.TryGetValue(dragon.SHA1Hash, out var cachedDragon))
            {
                using (var ctx = new DataContext())
                {
                    var dbDragon = ctx.DragonCache.FirstOrDefault(x => x.SHA1Hash == dragon.SHA1Hash);
                    if (dbDragon != null)
                        dragon = dbDragon;
                    else
                        ctx.DragonCache.Add(dragon);
                    Cache[dragon.SHA1Hash] = dragon;

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

                    if (dragon.Age == Age.Dragon)
                        ctx.SaveChanges();
                }
            }
            else
                dragon = cachedDragon;

            if ((regexParse = Regex.Match(dragonUrl, @"apparel=([\d,]*)")).Success)
                dragon.Apparel = regexParse.Groups[1].Value;

            if ((regexParse = Regex.Match(dragonUrl, @"skin=([\d]*)")).Success)
                dragon.Skin = int.Parse(regexParse.Groups[1].Value);

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
                return GeneratedFRHelpers.GenerateDragonImageUrl(GetRegexValue(Regex.Match(htmlPage, @"breed=(\d+)")), GetRegexValue(Regex.Match(htmlPage, @"gender=(\d+)")), 1,
                    GetRegexValue(Regex.Match(htmlPage, @"bodygene=(\d+)")), GetRegexValue(Regex.Match(htmlPage, @"body=(\d+)")),
                    GetRegexValue(Regex.Match(htmlPage, @"winggene=(\d+)")), GetRegexValue(Regex.Match(htmlPage, @"wings=(\d+)")),
                    GetRegexValue(Regex.Match(htmlPage, @"tertgene=(\d+)")), GetRegexValue(Regex.Match(htmlPage, @"tert=(\d+)")),
                    GetRegexValue(Regex.Match(htmlPage, @"element=(\d+)")), GetRegexValue(Regex.Match(htmlPage, @"eyetype=(\d+)")));
            }
        }

        private static int GetRegexValue(Match match) => int.Parse(match.Groups[1].Value);

        public static DragonCache GetDragonFromDragonId(int dragonId) => ParseUrlForDragon(GetDragonImageUrlFromDragonId(dragonId));

        public static Task<Bitmap> GetDragonBaseImage(string dragonUrl) => GetDragonBaseImage(ParseUrlForDragon(dragonUrl));

        public static async Task<Bitmap> GetDragonBaseImage(DragonCache dragon)
        {
            Bitmap dwagonImage;
            string azureUrl = $@"dragoncache\{dragon.SHA1Hash}.png";
            var azureImageService = new AzureFileService();

            if (await azureImageService.Exists(azureUrl) != null)
            {
                try
                {
                    using (var stream = await azureImageService.GetFile(azureUrl))
                        dwagonImage = (Bitmap)Image.FromStream(stream);
                    return dwagonImage;
                }
                catch
                {
                }
            }

            using (var client = new WebClient())
            {
                var dwagonImageBytes = await client.DownloadDataTaskAsync(dragon.ConstructImageString() ?? string.Format(DressingRoomDummyUrl, (int)dragon.DragonType, (int)dragon.Gender));
                using (var memStream = new MemoryStream(dwagonImageBytes, false))
                    await azureImageService.WriteFile(azureUrl, memStream);

                using (var memStream = new MemoryStream(dwagonImageBytes, false))
                {
                    dwagonImage = (Bitmap)Image.FromStream(memStream);
                    return dwagonImage;
                }
            }
        }

        public static async Task<FRUser> GetOrUpdateFRUser(string username, DataContext ctx = null) => await GetOrUpdateFRUser(username, null, ctx);
        public static async Task<FRUser> GetOrUpdateFRUser(int userId, DataContext ctx = null) => await GetOrUpdateFRUser(null, userId, ctx);


        private static async Task<FRUser> GetOrUpdateFRUser(string username, int? userId, DataContext ctx = null)
        {
            var dispose = false;
            if (dispose = ctx == null)
                ctx = new DataContext();

            Console.WriteLine($"Checking if we know {username ?? userId.ToString()}");

            var frUser = ctx.FRUsers.FirstOrDefault(x => x.Username == username || x.FRId == userId);

            if (frUser == null)
            {
                Console.WriteLine("We do not, fetching user info");
                var (frName, frId) = await GetFRUserInfo(username, userId);

                if (frName == null)
                {
                    var prev = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("User cannot be found");
                    Console.ForegroundColor = prev;
                    return null;
                }

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
                Console.WriteLine("We do, updating user");
                frUser = await frUser.UpdateFRUser();
            }

            if (dispose)
                ctx.Dispose();
            else
                await ctx.SaveChangesAsync();

            return frUser;
        }

        public static async Task<FRUser> UpdateFRUser(this FRUser frUser)
        {
            // Only update if it hasn't been a day to avoid spamming FR server
            if (DateTime.UtcNow < frUser.LastUpdated.AddDays(1))
            {
                Console.WriteLine("User recently updated, skipping");
                return frUser;
            }

            var (frName, frId) = await GetFRUserInfo(null, frUser.FRId);

            if (frName == null)
                return null;

            frUser.Username = frName;
            frUser.FRId = frId;
            frUser.LastUpdated = DateTime.UtcNow;
            await Task.Delay(50);
            return frUser;
        }


        private static async Task<(string Username, int UserId)> GetFRUserInfo(string username, int? userId)
        {
            string url = $"https://www1.flightrising.com/clan-profile/{(userId?.ToString() ?? $"n/{username}")}";
            using (var client = new WebClient())
            {
                var userProfilePage = await client.DownloadStringTaskAsync(url);
                if (userProfilePage.Contains("404 - Page Not Found") || userProfilePage.Contains("404: User not found"))
                    return default;

                var userBio = Regex.Match(userProfilePage, @"<div class=""userdata-section"" style=""height:136px;"">[\s\S]+?<span style=""position:absolute; top:8px; left:8px; color:#731d08; font-weight:bold; font-size:16px;"">\s+([\s\S]+?)\s+</span>[\s\S]+?<span>([\d]+?)</span>[\s\S]+?</div>");
                var frName = userBio.Groups[1].Value;
                var frId = int.Parse(userBio.Groups[2].Value);
                Console.WriteLine($"Found info:\n\tUsername: {frName}\n\tID: {frId}");

                return (frName, frId);
            }
        }

        public static Flight GetFlightFromGodName(string godName)
        {
            switch (godName.ToLower())
            {
                default:
                case "earthshaker":
                    return Flight.Earth;
                case "plaguebringer":
                    return Flight.Plague;
                case "windsinger":
                    return Flight.Wind;
                case "tidelord":
                    return Flight.Water;
                case "stormcatcher":
                    return Flight.Lightning;
                case "icewarden":
                    return Flight.Ice;
                case "shadowbinder":
                    return Flight.Shadow;
                case "lightweaver":
                    return Flight.Light;
                case "arcanist":
                    return Flight.Arcane;
                case "gladekeeper":
                    return Flight.Nature;
                case "flamecaller":
                    return Flight.Fire;
            }
        }

        public static async Task<FRItem> FetchItem(int itemId, string category = "skins")
        {
            var client = new HtmlWeb();
            var itemDoc = client.Load(string.Format(ItemFetchUrl, itemId, category));
            var iconUrl = itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/img[2]").GetAttributeValue("src", "/images/cms//.png");

            if (iconUrl == "/images/cms//.png")
            {
                Console.WriteLine($"Item {itemId} does not exist.");
                return null;
            }

            try
            {
                var categoryMatch = Regex.Match(iconUrl, @"/images/cms/(?<Category>.*)/(\d*).png");
                if (categoryMatch.Groups["Category"].Value != category)
                {
                    Console.WriteLine($"Item {itemId} is not {category}, but is actually {categoryMatch.Groups["Category"]}. Fetching that item instead.");
                    return await FetchItem(itemId, categoryMatch.Groups["Category"].Value);
                }
                using (var ctx = new DataContext())
                {
                    var existingItem = ctx.FRItems.FirstOrDefault(x => x.FRId == itemId);
                    if (existingItem != null)
                        ctx.FRItems.Remove(existingItem);
                    var item = ctx.FRItems.Add(new FRItem { FRId = itemId, IconUrl = iconUrl, ItemCategory = (FRItemCategory)Enum.Parse(typeof(FRItemCategory), category, true) });
                    item.Name = itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/div[1]").InnerText.Trim();
                    item.Description = CleanupFRHtmlText(itemDoc.DocumentNode.SelectSingleNode("/div/div[2]").InnerText);
                    item.ItemType = itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/div[2]").InnerText.Trim();
                    var rarityUrl = itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/img[1]").GetAttributeValue("src", "");
                    var rarityMatch = Regex.Match(rarityUrl, @"../images/layout/tooltips/star-(?<Rarity>\d).png");

                    if (rarityMatch.Success)
                        item.Rarity = int.Parse(rarityMatch.Groups["Rarity"].Value);

                    switch (item.ItemCategory)
                    {
                        case FRItemCategory.Food:
                            {
                                item.TreasureValue = int.TryParse(itemDoc.DocumentNode.SelectSingleNode("/div/div[3]").InnerText, out var treasureValue) ? (int?)treasureValue : null;
                                item.FoodValue = int.TryParse(itemDoc.DocumentNode.SelectSingleNode("/div/div[4]").InnerText, out var foodValue) ? (int?)foodValue : null;
                                item.FoodType = (FRFoodType)Enum.Parse(typeof(FRFoodType), item.ItemType, true);
                            }
                            break;
                        case FRItemCategory.Skins:
                            item.AssetUrl = itemDoc.DocumentNode.SelectSingleNode("/div/div[2]/div/img").GetAttributeValue("src", "");
                            var username = Regex.Match(item.Description, @"Designed by ([^.]+)[.|\)]");
                            if (username.Success)
                                item.Creator = await GetOrUpdateFRUser(username.Groups[1].Value, ctx);
                            break;
                        case FRItemCategory.Equipment:
                            {
                                item.TreasureValue = int.TryParse(itemDoc.DocumentNode.SelectSingleNode("/div/div[3]").InnerText, out var treasureValue) ? (int?)treasureValue : null;
                                item.AssetUrl = string.Format(DressingRoomDummyUrl, (int)DragonType.Fae, (int)Gender.Male) + $"&apparel=22046,{item.FRId}";
                            }
                            break;
                        case FRItemCategory.Familiar:
                            {
                                item.TreasureValue = int.TryParse(itemDoc.DocumentNode.SelectSingleNode("/div/div[3]").InnerText, out var treasureValue) ? (int?)treasureValue : null;
                                item.AssetUrl = string.Format(FamiliarArtUrl, item.FRId);
                            }
                            break;
                        case FRItemCategory.Battle_Items:
                            {
                                item.TreasureValue = int.TryParse(itemDoc.DocumentNode.SelectSingleNode("/div/div[3]").InnerText, out var treasureValue) ? (int?)treasureValue : null;
                                item.RequiredLevel = int.TryParse(itemDoc.DocumentNode.SelectSingleNode("/div/div[4]/strong").InnerText, out var requiredLevelValue) ? (int?)requiredLevelValue : null;
                            }
                            break;
                        case FRItemCategory.Trinket:
                            {
                                item.TreasureValue = int.TryParse(itemDoc.DocumentNode.SelectSingleNode("/div/div[3]").InnerText, out var treasureValue) ? (int?)treasureValue : null;
                                if (item.ItemType == "Scene")
                                    item.AssetUrl = string.Format(SceneArtUrl, item.FRId);
                            }
                            break;
                    }

                    item.AssetUrl = item.AssetUrl?.Replace("https://flightrising.com", "").Replace("https://www1.flightrising.com", "");
                    await ctx.SaveChangesAsync();
                    return item;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Item {itemId} threw error, possible deleted?");
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                Console.WriteLine($"Finished parsing item {itemId}");
                Console.WriteLine("--------------");
            }
        }

        public static string CleanupFRHtmlText(string input)
        {
            return input.Replace('\u000A', '\u0020')
                .Replace('\u0009', '\u0020')
                .Replace('\u000D', '\u0020')
                .Trim();
        }
    }
}