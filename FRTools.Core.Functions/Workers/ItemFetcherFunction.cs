using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Helpers;
using FRTools.Core.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FRTools.Core.Functions.Workers
{
    public class ItemFetcherFunction : FunctionBase
    {
        private readonly IAzureStorageService _azureStorage;
        private readonly IFRUserService _userService;
        private readonly IFRItemService _itemService;
        private readonly IAzurePipelineService _pipelineService;

        public ItemFetcherFunction(IAzureStorageService azureStorage, IFRUserService userService, IFRItemService itemService, IAzurePipelineService pipelineService)
        {
            _azureStorage = azureStorage;
            _userService = userService;
            _itemService = itemService;
            _pipelineService = pipelineService;
        }

        [FunctionName(nameof(ItemFetcher))]
        public async Task ItemFetcher([TimerTrigger("0 */15 * * * *", RunOnStartup = DEBUG)] TimerInfo timer, ILogger log)
        {
            var _noItemFoundCounter = 0;
            var lastRunPath = @"general-data\item-fetch\lastrun.json";

            log.LogInformation($"Timer trigger function FetchItems executed at: {DateTime.Now}");

            int maxTries = 3;
            if (await _azureStorage.Exists(lastRunPath))
            {
                using (var stream = await _azureStorage.GetFile(lastRunPath))
                using (var reader = new StreamReader(stream))
                {
                    var stringData = await reader.ReadToEndAsync();
                    var lastRunData = JsonConvert.DeserializeObject<dynamic>(stringData);
                    if (lastRunData != null)
                    {
                        var hasLastSuccess = DateTime.TryParse((string)lastRunData.LastSuccess, out DateTime lastSuccess);
                        if (hasLastSuccess && DateTime.UtcNow > lastSuccess.AddDays(1))
                            maxTries += (int)(DateTime.UtcNow - lastSuccess.AddDays(1)).TotalHours;

                        log.LogInformation($"Last succesful bout of skins were found at {lastSuccess}, which makes the max tries to be {maxTries} attempts");
                    }
                }
            }
            else
                log.LogWarning($"No last run found, max tries is {maxTries} attempts");

            var highestItemId = await _itemService.GetHighestItemId();
            var items = new List<FRItem>();
            while (_noItemFoundCounter < maxTries)
            {
                ++highestItemId;
                log.LogInformation($"Fetching item: {highestItemId}");
                var item = await _itemService.FetchItemFromFR(highestItemId);
                if (item != null)
                {
                    _noItemFoundCounter = 0;
                    items.Add(item);
                }
                else
                    _noItemFoundCounter++;
                await Task.Delay(100);
            }

            log.LogInformation($"Done for now, saving {items.Count} items.");

            if (items.Any())
            {
                using (var stream = new MemoryStream())
                using (var textWriter = new StreamWriter(stream))
                using (var writer = new JsonTextWriter(textWriter))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, new { items.Count, LastSuccess = DateTime.UtcNow });
                    await writer.FlushAsync();
                    stream.Position = 0;
                    await _azureStorage.CreateFile(lastRunPath, stream);
                }

                log.LogInformation($"Since items were found, saving last success at {DateTime.UtcNow}");

                log.LogInformation("Checking if we got any new genes or breeds");
                if (FRHelpers.CheckForUnknownGenesOrBreed(items))
                {
                    if(!DEBUG)
                        await _pipelineService.TriggerPipeline(Environment.GetEnvironmentVariable("AzureDevOpsPipeline"));
                }
            }

            if (DateTime.UtcNow.Date.Day == DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month) && DateTime.UtcNow.Hour == 23 && DateTime.UtcNow.Minute >= 45)
            {
                log.LogInformation("Once a month checking for missing ids");

                var missingIds = await _itemService.FindMissingIds();
                foreach (var missingId in missingIds)
                {
                    var item = await _itemService.FetchItemFromFR(missingId);
                }
            }
        }
    }
}
