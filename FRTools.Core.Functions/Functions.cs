using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Helpers;
using FRTools.Core.Services.Interfaces;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FRTools.Core.Functions
{
    public class Functions
    {

#if DEBUG
        private const bool DEBUG = true;
#else
        private const bool DEBUG = false;
#endif

        private readonly DataContext _dataContext;
        private readonly IAzureStorageService _azureStorage;
        private readonly IFRUserService _userService;
        private readonly IFRItemService _itemService;

        public Functions(DataContext dataContext, IAzureStorageService azureStorage, IFRUserService userService, IFRItemService itemService)
        {
            _dataContext = dataContext;
            _azureStorage = azureStorage;
            _userService = userService;
            _itemService = itemService;
        }

        [FunctionName("ItemFetcher")]
        public async Task FetchItems([TimerTrigger("0 */15 * * * *", RunOnStartup = DEBUG)] TimerInfo timer, ILogger log)
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

            var highestItemId = _dataContext.FRItems.Any() ? _dataContext.FRItems.Max(x => x.FRId) : 0;
            var items = new List<FRItem>();
            while (_noItemFoundCounter < maxTries)
            {
                ++highestItemId;
                log.LogInformation($"Fetching item: {highestItemId}");
                var item = await _itemService.FetchItem(highestItemId);
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
                    serializer.Serialize(writer, new { Count = items.Count, LastSuccess = DateTime.UtcNow });
                    await writer.FlushAsync();
                    stream.Position = 0;
                    await _azureStorage.CreateFile(lastRunPath, stream);
                }

                log.LogInformation($"Since items were found, saving last success at {DateTime.UtcNow}");

                //Console.WriteLine("Checking if we got any new genes or breeds");
                //if (FRHelpers.CheckForUnknownGenesOrBreed(items))
                //{
                //    AzurePipeLineService.TriggerRegenerateClassesPipeline();
                //}
            }

            if (DateTime.UtcNow.Date.Day == DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month) && DateTime.UtcNow.Hour == 23 && DateTime.UtcNow.Minute >= 45)
            {
                log.LogInformation("Once a month checking for missing ids");

                var missingIds = _dataContext.FRItems.FromSqlRaw("SELECT * FROM FRItems [first] WHERE NOT EXISTS (SELECT NULL FROM FRItems [second] WHERE [second].FRId = [first].FRId + 1) ORDER BY FRId").Select(x => x.FRId + 1).ToList();
                foreach (var missingId in missingIds)
                {
                    var item = await _itemService.FetchItem(missingId);
                }
            }
        }

        [FunctionName("FlashTracker")]
        public async Task FlashTracker([TimerTrigger("0 1 8,20 * * *", RunOnStartup = DEBUG)] TimerInfo timer, ILogger log)
        {
            string[] _tabs = { "apparel", "familiars", "specialty", "genes", "scenes", "skins", "battle", "bundles" };

            var marketPlaceDoc = await LoadHtmlPage(FRHelpers.MarketplaceUrl);
            var marketTabs = marketPlaceDoc.DocumentNode.SelectNodes("//*[@id=\"market-tabs\"]/div");
            var link = marketTabs.First(x => x.ChildNodes.Any(c => c.HasClass("flash_sale_tab_icon"))).SelectSingleNode("a").GetAttributeValue("href", null);

            var itemsDoc = await LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, link.Split('/').Last()));
            var items = itemsDoc.DocumentNode.SelectNodes("//*[@id=\"market-result-items-content\"]/span");
            var flashSaleItem = items.FirstOrDefault(x => x.HasClass("market-flash-result"));

            if (flashSaleItem == null)
            {
                foreach (var tab in _tabs)
                {
                    itemsDoc = await LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, tab));
                    items = itemsDoc.DocumentNode.SelectNodes("//*[@id=\"market-result-items-content\"]/span");
                    flashSaleItem = items.FirstOrDefault(x => x.HasClass("market-flash-result"));
                    if (flashSaleItem != null)
                        break;
                }
            }

            if (flashSaleItem == null)
                return;

            var itemId = int.Parse(flashSaleItem.ChildNodes.First(x => x.GetAttributes().Any(a => a.Name == "data-itemid")).GetAttributeValue("data-itemid", null));
            var item = _dataContext.FRItems.FirstOrDefault(x => x.FRId == itemId);
            if (item == null)
            {
                item = await _itemService.FetchItem(itemId);
            }
            if (item.FlashSales.All(x => x.RemovedAt != null))
            {
                try
                {
                    using (var tumblrClient = new TumblrClientFactory().Create<TumblrClient>(
                        Environment.GetEnvironmentVariable("TumblrClientId"),
                        Environment.GetEnvironmentVariable("TumblrSecret"),
                        new Token(
                        Environment.GetEnvironmentVariable("TumblrOAuthToken"),
                        Environment.GetEnvironmentVariable("TumblrOAuthSecret"))))
                    {
                        var tags = new List<string> { "frtools", "fr tools", "flight rising", "flightrising", "fr", "flash sale", "flashsale", item.Name.ToLower() };

                        string body = $"<p>A new flash sale has been discovered for <b>{item.Name}</b></p>";
                        body += $"<p><i>{item.Description}</i></p><br/>";
                        body += "<p>";
                        body += $"Game database: <a href=\"{string.Format(FRHelpers.GameDatabaseUrl, item.FRId)}\">click here</a><br/>";
                        body += $"Marketplace link: <a href=\"{link}\">click here</a><br/>";
                        body += "</p>";

                        if (item.TreasureValue > 0)
                            body += $"Treasure: <strike>{item.TreasureValue * 10}</strike> <b>{item.TreasureValue * .8 * 10}</b><br/>";

                        if (item.FoodValue > 0)
                            body += $"Food: {item.FoodValue}<br/>";

                        string itemUrl = null;
                        var random = new Random();
                        switch (item.ItemCategory)
                        {
                            case FRItemCategory.Skins:
                                var skinType = item.ItemType.Split(' ');
                                var dragonType = (DragonType)Enum.Parse(typeof(DragonType), skinType[0]);
                                var gender = (Gender)Enum.Parse(typeof(Gender), skinType[1]);
                                itemUrl = $"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/skin/{nameof(ProxyDummyDragonApparel)}/{(int)dragonType}/{(int)gender}/{item.FRId}";
                                body += $"For: {dragonType} {gender}<br/>";

                                var username = Regex.Match(item.Description, @"Designed by ([^.]+)[.|\)]");
                                if (username.Success)
                                {
                                    var frUser = await _userService.GetOrUpdateFRUser(username.Groups[1].Value);
                                    if (frUser != null)
                                        body += $"Created by: {frUser.Username}";
                                }

                                tags.Add("fr skins and accents");
                                tags.Add("fr skins");
                                tags.Add("fr accents");
                                tags.Add("fr skin");
                                tags.Add("fr accent");
                                tags.Add("skins and accents");
                                break;
                            case FRItemCategory.Equipment:
                                {
                                    var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                                    itemUrl = $"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/apparel/{nameof(ProxyDummyDragonApparel)}/{(int)modernBreeds[random.Next(1, modernBreeds.Length)]}/{random.Next(0, 2)}/{item.FRId}";
                                    tags.Add(item.ItemType.ToLower());
                                    break;
                                }
                            case FRItemCategory.Familiar:
                                itemUrl = string.Format(FRHelpers.FamiliarArtUrl, item.FRId);
                                tags.Add("fr familiar");
                                tags.Add("familiar");
                                break;
                            case FRItemCategory.Trinket when item.ItemType == "Specialty Item" && (item.Name.StartsWith("Primary") || item.Name.StartsWith("Secondary") || item.Name.StartsWith("Tertiary")):
                                tags.Add($"{(item.Name.StartsWith("Primary") ? "primary" : item.Name.StartsWith("Secondary") ? "secondary" : "tertiarty")} gene");
                                tags.Add("gene");
                                tags.Add(item.Name.Split(':')[1].ToLower());
                                var ancientBreed = GeneratedFRHelpers.GetAncientBreeds().Where(x => item.Name.EndsWith($"({x})"));
                                if (ancientBreed.Any())
                                {
                                    itemUrl = $"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/gene/{nameof(ProxyDummyDragonGene)}/{(int)ancientBreed.First()}/{random.Next(0, 2)}/{item.FRId}";
                                    tags.Add("ancient gene");
                                    tags.Add(ancientBreed.First().ToString().ToLower());
                                }
                                else
                                {
                                    var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                                    itemUrl = $"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/gene/{nameof(ProxyDummyDragonGene)}/{(int)modernBreeds[random.Next(1, modernBreeds.Length)]}/{random.Next(0, 2)}/{item.FRId}";
                                }
                                break;
                            default:
                                if (item.ItemType == "Scene")
                                {
                                    itemUrl = string.Format(FRHelpers.SceneArtUrl, item.FRId);
                                    tags.Add("scene");
                                }
                                else
                                {
                                    if (item.AssetUrl != null)
                                    {
                                        Console.WriteLine("Unknown art type, attempting AssetURL");
                                        itemUrl = $"https://www1.flightrising.com{item.AssetUrl}";
                                        tags.Add(item.ItemType.ToLower());
                                    }
                                }
                                break;
                        }

                        body += $"<img src=\"{itemUrl ?? $"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/icon/{item.FRId}"}\"/>";

                        var post = PostData.CreateText(body, $"New Flash Sale: {item.Name}", tags);
                        if(DEBUG)
                            post.State = PostCreationState.Private;
                        var postParamaters = (MethodParameterSet)typeof(PostData).GetField("parameters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(post);
                        postParamaters.Add("native_inline_images", true);
                        await tumblrClient.CreatePostAsync("frtools", post);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                item.FlashSales.Add(new FRItemFlashSale
                {
                    DiscoveredAt = DateTime.UtcNow
                });
                _dataContext.FRItemFlashSales.Where(x => x.Item.FRId != itemId && x.RemovedAt == null).ToList().ForEach(x => x.RemovedAt = DateTime.UtcNow);
                _dataContext.SaveChanges();
            }
        }

        private async Task<HtmlDocument> LoadHtmlPage(string url)
        {
            var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true });

            var response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.Found)
            {
                return await LoadHtmlPage(response.Headers.Location.AbsoluteUri);
            }

            var resultDocument = new HtmlDocument();
            resultDocument.Load(response.Content.ReadAsStream(), Encoding.GetEncoding("utf-8"));

            return resultDocument;
        }

        [FunctionName(nameof(ProxyDummyDragonSkin))]
        public async Task<IActionResult> ProxyDummyDragonSkin([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/dragon/skin/{dragonType:int}/{gender:int}/{skinId:int}")] HttpRequest req, int dragonType, int gender, int skinId, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            using (var client = new WebClient())
            {
                var apparelBytes = await client.DownloadDataTaskAsync(string.Format(FRHelpers.DressingRoomDummySkinUrl, dragonType, gender, skinId));

                return new FileContentResult(apparelBytes, "image/png");
            }
        }

        [FunctionName(nameof(ProxyDummyDragonGene))]
        public async Task<IActionResult> ProxyDummyDragonGene([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/dragon/gene/{dragonType:int}/{gender:int}/{geneId:int}")] HttpRequest req, int dragonType, int gender, int geneId, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var item = _dataContext.FRItems.FirstOrDefault(x => x.FRId == geneId);
            int primary = 0, secondary = 0, tertiary = 0;
            int primaryColor = 0, secondaryColor = 0, tertiaryColor = 0;
            var itemSplit = item.Name.Split(':', '(');
            var random = new Random();
            byte[] geneBytes;
            switch (itemSplit[0])
            {
                case "Primary Gene":
                    primary = (int)Enum.Parse(GeneratedFREnumExtentions.PrimaryGeneType((DragonType)dragonType), itemSplit[1].Replace("-", "").Replace(" ", ""));
                    primaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
                    break;
                case "Secondary Gene":
                    secondary = (int)Enum.Parse(GeneratedFREnumExtentions.SecondaryGeneType((DragonType)dragonType), itemSplit[1].Replace("-", "").Replace(" ", ""));
                    secondaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
                    break;
                case "Tertiary Gene":
                    tertiary = (int)Enum.Parse(GeneratedFREnumExtentions.TertiaryGeneType((DragonType)dragonType), itemSplit[1].Replace("-", "").Replace(" ", ""));
                    tertiaryColor = random.Next(0, Enum.GetValues(typeof(Color)).Length + 1);
                    break;
            }

            using (var client = new WebClient())
                geneBytes = await client.DownloadDataTaskAsync(GeneratedFRHelpers.GenerateDragonImageUrl(dragonType, gender, 1, primary, primaryColor, secondary, secondaryColor, tertiary, tertiaryColor, random.Next(0, Enum.GetValues(typeof(Element)).Length + 1), random.Next(0, Enum.GetValues(typeof(EyeType)).Length + 1)));

            return new FileContentResult(geneBytes, "image/png");
        }

        [FunctionName(nameof(ProxyDummyDragonApparel))]
        public async Task<IActionResult> ProxyDummyDragonApparel([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/dragon/apparel/{dragonType:int}/{gender:int}/{apparelId:int}")] HttpRequest req, int dragonType, int gender, int apparelId, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            using (var client = new WebClient())
            {
                var apparelBytes = await client.DownloadDataTaskAsync(string.Format(FRHelpers.DressingRoomDummyApparalUrl, dragonType, gender, apparelId));

                return new FileContentResult(apparelBytes, "image/png");
            }
        }

        [FunctionName(nameof(ProxyIcon))]
        public async Task<IActionResult> ProxyIcon([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "proxy/icon/{itemId:int}")] HttpRequest req, int itemId, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var item = _dataContext.FRItems.FirstOrDefault(x => x.FRId == itemId);
            if (item == null)
                return null;

            using (var client = new WebClient())
            {
                var bytes = await client.DownloadDataTaskAsync("https://flightrising.com" + item.IconUrl);
                return new FileContentResult(bytes, "image/png");
            }
        }
    }
}
