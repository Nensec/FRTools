using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRTools.Common;
using FRTools.Data;
using FRTools.Data.Messages;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace FRTools.MS.ItemFetcher
{
    class Program
    {
        private const string lastRunPath = @"general-data\item-fetch\lastrun.json";
        private static IQueueClient _serviceBus;
        private static int _noItemFoundCounter = 0;

        static async Task Main()
        {
            int maxTries = 3;
            var azureFileService = new AzureFileService();
            if (await azureFileService.Exists(lastRunPath) != null)
            {
                using (var stream = await azureFileService.GetFile(lastRunPath))
                using (var reader = new StreamReader(stream))
                {
                    var stringData = await reader.ReadToEndAsync();
                    var lastRunData = JsonConvert.DeserializeObject<dynamic>(stringData);
                    if (lastRunData != null)
                    {
                        var hasLastSuccess = DateTime.TryParse((string)lastRunData.LastSuccess, out DateTime lastSuccess);
                        if (hasLastSuccess && DateTime.UtcNow > lastSuccess.AddDays(1))
                            maxTries += (int)(DateTime.UtcNow - lastSuccess.AddDays(1)).TotalHours;

                        Console.WriteLine($"Last succesful bout of skins were found at {lastSuccess}, which makes the max tries to be {maxTries} attempts");
                    }
                }
            }
            else
                Console.WriteLine($"No last run found, max tries is {maxTries} attempts");
            _serviceBus = new QueueClient(ConfigurationManager.AppSettings["AzureSBConnString"], ConfigurationManager.AppSettings["AzureSBQueueName"]);
            await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GenericMessage(MessageCategory.ItemFetcher, "Started")))));

            var random = new Random();
            using (var ctx = new DataContext())
            {
                var highestItemId = ctx.FRItems.Any() ? ctx.FRItems.Max(x => x.FRId) : 0;
                var count = 0;
                while (_noItemFoundCounter < maxTries)
                {
                    ++highestItemId;
                    Console.WriteLine($"Fetching item: {highestItemId}");
                    var item = await FRHelpers.FetchItem(highestItemId);
                    if (item != null)
                    {
                        _noItemFoundCounter = 0;
                        count++;
                        await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NewItemMessage(MessageCategory.ItemFetcher, item)))));
                    }
                    else
                        _noItemFoundCounter++;
                    await Task.Delay(100);
                }

                Console.WriteLine($"Done for now, saving {count} items.");
                await ctx.SaveChangesAsync();

                if (count > 0)
                {
                    using (var stream = new MemoryStream())
                    using (var textWriter = new StreamWriter(stream))
                    using (var writer = new JsonTextWriter(textWriter))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(writer, new { Count = count, LastSuccess = DateTime.UtcNow });
                        await writer.FlushAsync();
                        stream.Position = 0;
                        await azureFileService.WriteFile(lastRunPath, stream);
                    }

                    Console.WriteLine($"Since skins were found, saving last success at {DateTime.UtcNow}");
                }
            }
            await _serviceBus.CloseAsync();
        }
    }
}
