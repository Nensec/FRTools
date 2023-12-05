using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Helpers;
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

        public FlashSaleTrackerFunction(DataContext dataContext, IFRUserService userService, IFRItemService itemService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _itemService = itemService;
        }

        [FunctionName(nameof(FlashTracker))]
        public async Task FlashTracker([TimerTrigger("0 1 8,20 * * *", RunOnStartup = DEBUG)] TimerInfo timer, ILogger log)
        {
            string[] _tabs = { "apparel", "familiars", "specialty", "genes", "scenes", "skins", "battle", "bundles" };

            var marketPlaceDoc = await Common.Helpers.LoadHtmlPage(FRHelpers.MarketplaceUrl);
            var marketTabs = marketPlaceDoc.DocumentNode.SelectNodes("//*[@id=\"market-tabs\"]/div");
            var link = marketTabs.First(x => x.ChildNodes.Any(c => c.HasClass("flash_sale_tab_icon"))).SelectSingleNode("a").GetAttributeValue("href", null);

            var itemsDoc = await Common.Helpers.LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, link.Split('/').Last()));
            var items = itemsDoc.DocumentNode.SelectNodes("//*[@id=\"market-result-items-content\"]/span");
            var flashSaleItem = items.FirstOrDefault(x => x.HasClass("market-flash-result"));

            if (flashSaleItem == null)
            {
                foreach (var tab in _tabs)
                {
                    itemsDoc = await Common.Helpers.LoadHtmlPage(string.Format(FRHelpers.MarketplaceFetchUrl, tab));
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
                                itemUrl = $"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/skin/{(int)dragonType}/{(int)gender}/{item.FRId}";
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
                                    itemUrl = $"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/apparel/{(int)modernBreeds[random.Next(1, modernBreeds.Length)]}/{random.Next(0, 2)}/{item.FRId}";
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
                                    itemUrl = $"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/gene/{(int)ancientBreed.First()}/{random.Next(0, 2)}/{item.FRId}";
                                    tags.Add("ancient gene");
                                    tags.Add(ancientBreed.First().ToString().ToLower());
                                }
                                else
                                {
                                    var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                                    itemUrl = $"https://{Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")}/api/proxy/dragon/gene/{(int)modernBreeds[random.Next(1, modernBreeds.Length)]}/{random.Next(0, 2)}/{item.FRId}";
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
                        if (DEBUG)
                            post.State = PostCreationState.Private;
                        var postParamaters = (MethodParameterSet)typeof(PostData).GetField("parameters", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(post);
                        postParamaters.Add("native_inline_images", true);
                        await tumblrClient.CreatePostAsync("frtools", post);
                    }
                }
                catch (Exception ex)
                {
                    log.LogError(ex.ToString());
                }

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
