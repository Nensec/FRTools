using System;
using System.Linq;
using System.Threading.Tasks;
using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack.CssSelectors.NetCore;
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
        private readonly IHtmlService _htmlService;

        public FlashSaleTrackerFunction(DataContext dataContext, IFRUserService userService, IFRItemService itemService, IAnnounceService announceService, IHtmlService htmlService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _itemService = itemService;
            _announceService = announceService;
            _htmlService = htmlService;
        }

        [FunctionName(nameof(FlashTracker))]
        public async Task FlashTracker([TimerTrigger("0 1 7,19 * * *", RunOnStartup = DEBUG)] TimerInfo timer, ILogger log)
        {
            var marketPlaceDoc = await _htmlService.LoadHtmlPage(FRHelpers.MarketplaceUrl);
            var marketTabs = marketPlaceDoc.DocumentNode.QuerySelectorAll(".market-tab .common-tab");
            var _tabs = marketTabs.Select(x => x.SelectSingleNode("a").GetAttributeValue("href", null).Split('/').Last()).ToArray();
            var link = marketTabs.First(x => x.ChildNodes.Any(c => c.HasClass("flash_sale_tab_icon"))).SelectSingleNode("a").GetAttributeValue("href", null);

            var itemsDoc = await _htmlService.LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, link.Split('/').Last()));
            var items = itemsDoc.DocumentNode.QuerySelectorAll(".market-item-result");
            var flashSaleItem = items.FirstOrDefault(x => x.HasClass("market-flash-result"));

            if (flashSaleItem == null)
            {
                foreach (var tab in _tabs)
                {
                    itemsDoc = await _htmlService.LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, tab));
                    items = itemsDoc.DocumentNode.QuerySelectorAll(".market-item-result");
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