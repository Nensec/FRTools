using System.Text.RegularExpressions;
using System.Web;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Helpers;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services
{
    public class FRItemService : IFRItemService
    {
        private readonly DataContext _dataContext;
        private readonly IFRUserService _userService;
        private readonly ILogger _logger;

        public FRItemService(DataContext dataContext, IFRUserService userService, ILogger logger)
        {
            _dataContext = dataContext;
            _userService = userService;
            _logger = logger;
        }

        public async Task<FRItem> FetchItem(int itemId, string category = "skins")
        {
            var client = new HtmlWeb();
            var itemDoc = client.Load(string.Format(FRHelpers.ItemFetchUrl, itemId, category));
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

                var existingItem = _dataContext.FRItems.FirstOrDefault(x => x.FRId == itemId);
                if (existingItem != null)
                    _dataContext.FRItems.Remove(existingItem);
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
                            item.Creator = await _userService.GetOrUpdateFRUser(username.Groups[1].Value, _dataContext);
                        break;
                    case FRItemCategory.Equipment:
                        {
                            item.TreasureValue = int.TryParse(itemDoc.DocumentNode.SelectSingleNode("/div/div[3]").InnerText, out var treasureValue) ? (int?)treasureValue : null;
                            item.AssetUrl = string.Format(FRHelpers.DressingRoomDummyUrl, (int)DragonType.Fae, (int)Gender.Male) + $"&apparel=22046,{item.FRId}";
                        }
                        break;
                    case FRItemCategory.Familiar:
                        {
                            item.TreasureValue = int.TryParse(itemDoc.DocumentNode.SelectSingleNode("/div/div[3]").InnerText, out var treasureValue) ? (int?)treasureValue : null;
                            item.AssetUrl = string.Format(FRHelpers.FamiliarArtUrl, item.FRId);
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
                                item.AssetUrl = string.Format(FRHelpers.SceneArtUrl, item.FRId);
                        }
                        break;
                }

                item.AssetUrl = item.AssetUrl?.Replace("https://flightrising.com", "").Replace("https://www1.flightrising.com", "");
                _dataContext.FRItems.Add(item);
                await _dataContext.SaveChangesAsync();
                return item;
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
    }
}