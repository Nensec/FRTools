using System;
using System.Linq;
using System.Threading.Tasks;
using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Functions.Workers
{
    public class FlashSaleTrackerFunction : FunctionBase
    {
        private readonly DataContext _dataContext;
        private readonly IFRUserService _userService;
        private readonly IFRItemService _itemService;
        private readonly IAnnounceService _announceService;

        public FlashSaleTrackerFunction(DataContext dataContext, IFRUserService userService, IFRItemService itemService, IAnnounceService announceService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _itemService = itemService;
            _announceService = announceService;
        }

        [FunctionName(nameof(FlashTracker))]
        public async Task FlashTracker([TimerTrigger("0 1 8,20 * * *", RunOnStartup = DEBUG)] TimerInfo timer, ILogger log)
        {
            string[] _tabs = { "apparel", "familiars", "specialty", "genes", "scenes", "skins", "battle", "bundles" };

            var marketPlaceDoc = await Helpers.LoadHtmlPage(FRHelpers.MarketplaceUrl);
            var marketTabs = marketPlaceDoc.DocumentNode.SelectNodes("//*[@id=\"market-tabs\"]/div");
            var link = marketTabs.First(x => x.ChildNodes.Any(c => c.HasClass("flash_sale_tab_icon"))).SelectSingleNode("a").GetAttributeValue("href", null);

            var itemsDoc = await Helpers.LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, link.Split('/').Last()));
            var items = itemsDoc.DocumentNode.SelectNodes("//*[@id=\"market-result-items-content\"]/span");
            var flashSaleItem = items.FirstOrDefault(x => x.HasClass("market-flash-result"));

            if (flashSaleItem == null)
            {
                foreach (var tab in _tabs)
                {
                    itemsDoc = await Helpers.LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, tab));
                    items = itemsDoc.DocumentNode.SelectNodes("//*[@id=\"market-result-items-content\"]/span");
                    flashSaleItem = items.FirstOrDefault(x => x.HasClass("market-flash-result"));
                    if (flashSaleItem != null)
                        break;
                }
            }

            if (flashSaleItem == null)
                return;

            var itemId = int.Parse(flashSaleItem.ChildNodes.First(x => x.GetAttributes().Any(a => a.Name == "data-itemid")).GetAttributeValue("data-itemid", null));
            var item = await _itemService.GetItem(itemId) ?? await _itemService.FetchItemFromFR(itemId);
            if (item.FlashSales.All(x => x.RemovedAt != null))
            {
                await _announceService.Announce(new FlashSaleAnnounceData(item, link));

                item.FlashSales.Add(new FRItemFlashSale
                {
                    DiscoveredAt = DateTime.UtcNow
                });
                _dataContext.FRItemFlashSales.Where(x => x.Item.FRId != itemId && x.RemovedAt == null).ToList().ForEach(x => x.RemovedAt = DateTime.UtcNow);
                _dataContext.SaveChanges();
            }
        }
    }
}
