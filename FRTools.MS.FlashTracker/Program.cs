using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Data.Messages;
using HtmlAgilityPack;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.MS.FlashTracker
{
    class Program
    {
        private static IQueueClient _serviceBus;

        static async Task Main()
        {
            _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
            await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.FlashTracker, "Started")))));

            var client = new HtmlWeb();
            var marketTabs = client.Load(string.Format(FRHelpers.MarketplaceUrl, "")).DocumentNode.SelectNodes("//*[@id=\"market-tabs\"]/div");
            var link = marketTabs.First(x => x.ChildNodes.Any(c => c.HasClass("flash_sale_tab_icon"))).SelectSingleNode("a").GetAttributeValue("href", null);

            var itemsDoc = client.Load(string.Format(FRHelpers.MarketplaceFetchUrl, link.Split('/').Last()));
            var items = itemsDoc.DocumentNode.SelectNodes("//*[@id=\"market-result-items-content\"]/span");
            var flashSaleItem = items.First(x => x.HasClass("market-flash-result"));
            var itemId = int.Parse(flashSaleItem.ChildNodes.First(x => x.GetAttributes().Any(a => a.Name == "data-itemid")).GetAttributeValue("data-itemid", null));
            using (var ctx = new DataContext())
            {
                var item = ctx.FRItems.First(x => x.FRId == itemId);
                if (item.FlashSales.All(x => x.RemovedAt != null))
                {
                    await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new FlashSaleMessage(MessageCategory.FlashTracker, item)))));
                    item.FlashSales.Add(new FRItemFlashSale
                    {
                        Item = ctx.FRItems.First(x => x.FRId == itemId),
                        DiscoveredAt = DateTime.UtcNow
                    });
                    ctx.FRItemFlashSales.Where(x => x.Item.FRId != itemId && x.RemovedAt == null).ToList().ForEach(x => x.RemovedAt = DateTime.UtcNow);
                    ctx.SaveChanges();
                }
            }
        }
    }
}