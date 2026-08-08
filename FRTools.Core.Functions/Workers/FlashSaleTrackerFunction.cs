using System;
using System.Linq;
using System.Threading.Tasks;
using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.Azure.Functions.Worker;
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
        private readonly ILogger<FlashSaleTrackerFunction> _logger;

        public FlashSaleTrackerFunction(DataContext dataContext, IFRUserService userService, IFRItemService itemService, IAnnounceService announceService, IHtmlService htmlService, ILogger<FlashSaleTrackerFunction> logger)
        {
            _dataContext = dataContext;
            _userService = userService;
            _itemService = itemService;
            _announceService = announceService;
            _htmlService = htmlService;
            _logger = logger;
        }

        [Function(nameof(FlashTracker))]
        public async Task FlashTracker([TimerTrigger("0 1 8,20 * * *", RunOnStartup = DEBUG)] TimerInfo timer)
        {
            var url = string.Format(FRHelpers.MarketplaceUrl, "apparel");
            var marketPlaceDoc = await _htmlService.LoadHtmlPage(url);
            var marketTabs = marketPlaceDoc.DocumentNode.QuerySelectorAll(".market-tab .common-tab");
            var currentTabIsFlash = marketTabs.Any(x => x.Name == "span" && x.ChildNodes.Any(c => c.HasClass("flash_sale_tab_icon")));
            var _tabs = marketTabs.Where(x => x.Name == "a").Select(x => x.GetAttributeValue("href", null!)?.Split('/').Last()).ToArray();

            if (!currentTabIsFlash)
                url = marketTabs.First(x => x.ChildNodes.Any(c => c.HasClass("flash_sale_tab_icon"))).GetAttributeValue("href", null!);

            marketPlaceDoc = await _htmlService.LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, url.Split('/').Last()));

            var items = marketPlaceDoc.DocumentNode.QuerySelectorAll(".market-item-result");
            var flashSaleItem = items.FirstOrDefault(x => x.HasClass("market-flash-result"));

            if (flashSaleItem == null)
            {
                foreach (var tab in _tabs)
                {
                    marketPlaceDoc = await _htmlService.LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, tab));
                    items = marketPlaceDoc.DocumentNode.QuerySelectorAll(".market-item-result");
                    flashSaleItem = items.FirstOrDefault(x => x.HasClass("market-flash-result"));
                    if (flashSaleItem != null)
                        break;
                }
            }

            if (flashSaleItem == null)
                return;

            var itemId = int.Parse(flashSaleItem.ChildNodes.First(x => x.GetAttributes().Any(a => a.Name == "data-itemid")).GetAttributeValue("data-itemid", null!));
            var item = await _itemService.GetItem(itemId) ?? await _itemService.FetchItemFromFR(itemId);
            if (item != null && item.FlashSales.All(x => x.RemovedAt != null))
            {
                await _announceService.Announce(new FlashSaleAnnounceData(item, url));

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