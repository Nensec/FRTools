using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FRTools.Core.Common;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Announce;
using FRTools.Core.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FRTools.Core.Functions.Workers
{
    public class ItemFetcherFunction : FunctionBase
    {
        private readonly IAzureStorageService _azureStorage;
        private readonly IFRItemService _itemService;
        private readonly IAzurePipelineService _pipelineService;
        private readonly IAnnounceService _announceService;
        private readonly ILogger<ItemFetcherFunction> _logger;
        private readonly int _fetchDelay = int.Parse(Environment.GetEnvironmentVariable("QueryFRDelay") ?? "0");

        public ItemFetcherFunction(IAzureStorageService azureStorage, IFRItemService itemService, IAzurePipelineService pipelineService, IAnnounceService announceService, ILogger<ItemFetcherFunction> logger)
        {
            _azureStorage = azureStorage;
            _itemService = itemService;
            _pipelineService = pipelineService;
            _announceService = announceService;
            _logger = logger;
        }

        [Function(nameof(ItemFetcher))]
        public async Task ItemFetcher([TimerTrigger("0 */15 * * * *", RunOnStartup = DEBUG)] TimerInfo timer)
        {
            var _noItemFoundCounter = 0;
            var lastRunPath = @"general-data\item-fetch\lastrun.json";

            _logger.LogInformation($"Timer trigger function FetchItems executed at: {DateTime.Now}");

            int maxTries = 3;
            if (await _azureStorage.Exists(lastRunPath))
            {
                using (var stream = await _azureStorage.GetFile(lastRunPath))
                using (var reader = new StreamReader(stream))
                {
                    var stringData = await reader.ReadToEndAsync();
                    var lastRunData = JsonConvert.DeserializeObject<LastRunData>(stringData);
                    if (lastRunData != null)
                    {
                        if (DateTime.UtcNow > lastRunData.LastSuccess.AddDays(1))
                            maxTries += (int)(DateTime.UtcNow - lastRunData.LastSuccess.AddDays(1)).TotalHours;

                        _logger.LogInformation($"Last succesful bout of skins were found at {lastRunData.LastSuccess}, which makes the max tries to be {maxTries} attempts");
                    }
                }
            }
            else
                _logger.LogWarning($"No last run found, max tries is {maxTries} attempts");

            var highestItemId = await _itemService.GetHighestItemId();
            var items = new List<FRItem>();
            while (_noItemFoundCounter < maxTries)
            {
                ++highestItemId;
                _logger.LogInformation($"Fetching item: {highestItemId}");
                var item = await _itemService.FetchItemFromFR(highestItemId);
                if (item != null)
                {
                    _noItemFoundCounter = 0;
                    items.Add(item);
                }
                else
                    _noItemFoundCounter++;
                await Task.Delay(_fetchDelay);
            }

            _logger.LogInformation($"Done for now, saving {items.Count} items.");

            if (items.Any())
            {
                await _announceService.Announce(new NewItemsAnnounceData(items));

                using (var stream = new MemoryStream())
                using (var textWriter = new StreamWriter(stream))
                using (var writer = new JsonTextWriter(textWriter))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, new LastRunData { Count = items.Count, LastSuccess = DateTime.UtcNow });
                    await writer.FlushAsync();
                    stream.Position = 0;
                    await _azureStorage.CreateOrUpdateFile(lastRunPath, stream);
                }

                _logger.LogInformation($"Since items were found, saving last success at {DateTime.UtcNow}");

                _logger.LogInformation("Checking if we got any new genes or breeds");
                if (!DEBUG && FRHelpers.CheckForUnknownGenesOrBreed(items))
                    await _pipelineService.TriggerPipeline(Environment.GetEnvironmentVariable("AzureDevOpsPipeline"));
            }

            if (DateTime.UtcNow.Date.Day == DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month) && DateTime.UtcNow.Hour == 23 && DateTime.UtcNow.Minute >= 45)
            {
                _logger.LogInformation("Once a month checking for missing ids");

                var missingItems = new List<FRItem>();
                var missingIds = await _itemService.FindMissingIds();

                foreach (var missingId in missingIds)
                {
                    var newItem = await _itemService.FetchItemFromFR(missingId);
                    if (newItem != null)
                        missingItems.Add(newItem);
                }

                if (missingItems.Any())
                    await _announceService.Announce(new NewItemsAnnounceData(missingItems));

            }
        }

        class LastRunData
        {
            public int Count { get; set; }
            public DateTime LastSuccess { get; set; }
        }
    }
}
