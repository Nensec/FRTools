using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services
{
    public class FRItemService : IFRItemService
    {
        private readonly DataContext _dataContext;
        private readonly IFRUserService _userService;
        private readonly ILogger<FRItemService> _logger;

        public FRItemService(DataContext dataContext, IFRUserService userService, ILogger<FRItemService> logger)
        {
            _dataContext = dataContext;
            _userService = userService;
            _logger = logger;
        }

        public async Task<FRItem?> FetchItemFromFR(int itemId, string category = "skins")
        {
            var client = new HtmlWeb();
            var itemDoc = client.Load(string.Format(FRHelpers.ItemFetchUrl, itemId, category));
            var iconUrl = itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/img[2]").GetAttributeValue("src", "/images/cms//.png");

            if (iconUrl == "/images/cms//.png")
            {
                _logger.LogInformation($"Item {itemId} does not exist.");
                return null;
            }

            try
            {
                var categoryMatch = Regex.Match(iconUrl, @"/images/cms/(?<Category>.*)/(\d*).png");
                if (categoryMatch.Groups["Category"].Value != category)
                {
                    _logger.LogInformation($"Item {itemId} is not {category}, but is actually {categoryMatch.Groups["Category"]}. Fetching that item instead.");
                    return await FetchItemFromFR(itemId, categoryMatch.Groups["Category"].Value);
                }

                var item = _dataContext.FRItems.FirstOrDefault(x => x.FRId == itemId) ?? _dataContext.FRItems.Add(new FRItem()).Entity;

                item.FRId = itemId;
                item.IconUrl = iconUrl;
                item.ItemCategory = Enum.Parse<FRItemCategory>(category, true);
                item.Name = HttpUtility.HtmlDecode(itemDoc.DocumentNode.SelectSingleNode("/div/div[1]/div[1]").InnerText.Trim());
                item.Description = HttpUtility.HtmlDecode(FRHelpers.CleanupFRHtmlText(itemDoc.DocumentNode.SelectSingleNode("/div/div[2]").InnerText));
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
                            item.FoodType = Enum.Parse<FRFoodType>(item.ItemType, true);
                        }
                        break;
                    case FRItemCategory.Skins:
                        item.AssetUrl = itemDoc.DocumentNode.SelectSingleNode("/div/div[2]/div/img").GetAttributeValue("src", "");
                        var username = Regex.Match(item.Description, @"Designed by ([^.]+)[.|\)]");
                        if (username.Success)
                            item.Creator = await _userService.GetOrUpdateFRUser(username.Groups[1].Value);
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
                await _dataContext.SaveChangesAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Item {itemId} threw error, possible deleted?");
                _logger.LogWarning(ex.ToString());
                return null;
            }
            finally
            {
                _logger.LogInformation($"Finished parsing item {itemId}");
            }
        }

        public async Task<int> GetHighestItemId() => await _dataContext.FRItems.AnyAsync() ? await _dataContext.FRItems.MaxAsync(x => x.FRId) : 0;

        public Task<List<int>> FindMissingIds() => Task.Run(() => _dataContext.FRItems.FromSqlRaw("SELECT * FROM FRItems [first] WHERE NOT EXISTS (SELECT NULL FROM FRItems [second] WHERE [second].FRId = [first].FRId + 1) ORDER BY FRId").Select(x => x.FRId + 1).ToList());

        public async Task<FRItem?> GetItem(int itemId) => await _dataContext.FRItems.FirstOrDefaultAsync(x => x.FRId == itemId);

        public async Task<IEnumerable<FRItem>> SearchItems(Expression<Func<FRItem, bool>> expression) => await _dataContext.FRItems.Where(expression).ToListAsync();
    }
}