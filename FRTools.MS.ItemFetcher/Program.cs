using FRTools.Common;
using FRTools.Data;
using FRTools.Data.Messages;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
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
            var settings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            int maxTries = 3;

            if (File.Exists("lastrun.json"))
            {
                var lastRunData = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText("lastrun.json"));
                var hasLastSuccess = DateTime.TryParse((string)lastRunData.LastSuccess, out DateTime lastSuccess);
                if (hasLastSuccess && DateTime.UtcNow > lastSuccess.AddDays(1))
                {
                    maxTries += (int)(DateTime.UtcNow - lastSuccess.AddDays(1)).TotalHours;
                    Console.WriteLine($"Last succesful bout of skins were found at {lastSuccess}, which makes the max tries to be {maxTries} attempts");
                }
            }
            else
                Console.WriteLine($"No last run found, max tries is {maxTries} attempts");

            _serviceBus = new QueueClient(settings.AppSettings.Settings["AzureSBConnString"].Value, settings.AppSettings.Settings["AzureSBQueueName"].Value);
            await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.ItemFetcher, "Started")))));

            var random = new Random();
            using (var ctx = new DataContext())
            {
                var highestItemId = ctx.FRItems.Any() ? ctx.FRItems.Max(x => x.FRId) : 0;

                while (_noItemFoundCounter < maxTries)
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
                    await Task.Delay(100);
                }

                var count = ctx.ChangeTracker.Entries().Count();
                Console.WriteLine($"Done for now, saving {count} items.");
                await ctx.SaveChangesAsync();

                if (count > 0)
                {

                    File.WriteAllText("lastrun.json", JsonConvert.SerializeObject(new { Count = count, LastSuccess = DateTime.UtcNow }));
                    Console.WriteLine($"Since skins were found, saving last success at {DateTime.UtcNow}");
                    settings.Save();
                }
            }
            await _serviceBus.CloseAsync();
        }
    }
}
