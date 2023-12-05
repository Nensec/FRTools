using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Web;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Data;
using FRTools.Core.Common;

namespace FRTools.Core.Helpers
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
                    var item = new FRItem
                    {
                        FRId = itemId,
                        IconUrl = iconUrl,
                        ItemCategory = (FRItemCategory)Enum.Parse(typeof(FRItemCategory), category, true),
                        Name = HttpUtility.HtmlDecode(itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/div[1]").InnerText.Trim()),
                        Description = HttpUtility.HtmlDecode(CleanupFRHtmlText(itemDoc.DocumentNode.SelectSingleNode("/div/div[2]").InnerText)),
                        ItemType = itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/div[2]").InnerText.Trim()
                    };
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
                    ctx.FRItems.Add(item);
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

        private static string CleanupFRHtmlText(string input)
        {
            return input.Replace('\u000A', '\u0020')
                .Replace('\u0009', '\u0020')
                .Replace('\u000D', '\u0020')
                .Trim();
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

                frUser = ctx.FRUsers.FirstOrDefault(x => x.Username == frName || x.FRId == frId);
                if (frUser == default)
                {
                    frUser = new FRUser();
                    ctx.FRUsers.Add(frUser);
                }
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
            var client = new HtmlWeb();
            var userProfilePage = client.Load(url);

            if (userProfilePage.ParsedText.Contains("404 - Page Not Found") || userProfilePage.ParsedText.Contains("404: User not found"))
                return default;

            var userBio = userProfilePage.DocumentNode.QuerySelector(".clan-profile-user-frame");
            var frId = int.Parse(userBio.QuerySelectorAll(".clan-profile-stats .clan-profile-stat").First(x => x.QuerySelector("strong").InnerText == "Player ID").LastChild.InnerText.Trim());
            var frName = userBio.QuerySelector(".clan-profile-username").InnerText.Trim();
            Console.WriteLine($"Found info:\n\tUsername: {frName}\n\tID: {frId}");

            return (frName, frId);
        }
    }
}
