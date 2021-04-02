using FRTools.Common;
using FRTools.Data;
using FRTools.Data.Messages;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.MS.ItemFetcher
{
    class Program
    {
        private static IQueueClient _serviceBus;
        private static int _noItemFoundCounter = 0;

        static async Task Main()
        {
            _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
            await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.ItemFetcher, "Started")))));

            var random = new Random();
            using (var ctx = new DataContext())
            {
                var highestItemId = ctx.FRItems.Any() ? ctx.FRItems.Max(x => x.FRId) : 0;

                while (_noItemFoundCounter < 5)
                {
                    ++highestItemId;
                    Console.WriteLine($"Fetching item: {highestItemId}");
                    var item = FRHelpers.FetchItem(highestItemId);
                    if (item != null)
                    {
                        _noItemFoundCounter = 0;
                        ctx.FRItems.Add(item);
                        await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NewItemMessage(MessageCategory.ItemFetcher, item)))));
                    }
                    else
                        _noItemFoundCounter++;
                    await Task.Delay(random.Next(75, 500));
                }

                Console.WriteLine($"Done for now, saving {ctx.ChangeTracker.Entries().Count()} items.");
                await ctx.SaveChangesAsync();
            }
            await _serviceBus.CloseAsync();
        }
    }
}
