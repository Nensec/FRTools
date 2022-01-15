using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using FRTools.Common;
using FRTools.Data;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Data.Messages;
using HtmlAgilityPack;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

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
                var item = ctx.FRItems.FirstOrDefault(x => x.FRId == itemId);
                if (item == null)
                {
                    item = await FRHelpers.FetchItem(itemId);
                    if (item != null)
                        await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NewItemMessage(MessageCategory.FlashTracker, item)))));
                }
                if (item.FlashSales.All(x => x.RemovedAt != null))
                {
                    await _serviceBus.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new FlashSaleMessage(MessageCategory.FlashTracker, item), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }))));
                    try
                    {
                        using (var tumblrClient = new TumblrClientFactory().Create<TumblrClient>(
                            ConfigurationManager.AppSettings["TumblrClientId"],
                            ConfigurationManager.AppSettings["TumblrSecret"],
                            new Token(
                            ConfigurationManager.AppSettings["TumblrOAuthToken"],
                            ConfigurationManager.AppSettings["TumblrOAuthSecret"])))
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
                                    itemUrl = string.Format(SiteHelpers.DummyDragonSkinProxyUrl, (int)dragonType, (int)gender, $"{item.FRId}");
                                    body += $"For: {dragonType} {gender}<br/>";

                                    var username = Regex.Match(item.Description, @"Designed by ([^.]+)[.|\)]");
                                    if (username.Success)
                                    {
                                        var frUser = await FRHelpers.GetOrUpdateFRUser(username.Groups[1].Value);
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
                                        itemUrl = string.Format(SiteHelpers.DummyDragonApparelProxyUrl, (int)modernBreeds[random.Next(1, modernBreeds.Length)], random.Next(0, 2), $"{item.FRId}");
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
                                        itemUrl = string.Format(SiteHelpers.DummyDragonGeneProxyUrl, (int)ancientBreed.First(), random.Next(0, 2), $"{item.FRId}");
                                        tags.Add("ancient gene");
                                        tags.Add(ancientBreed.First().ToString().ToLower());
                                    }
                                    else
                                    {
                                        var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                                        itemUrl = string.Format(SiteHelpers.DummyDragonGeneProxyUrl, (int)modernBreeds[random.Next(1, modernBreeds.Length)], random.Next(0, 2), $"{item.FRId}");
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

                            body += $"<img src=\"{itemUrl ?? string.Format(SiteHelpers.IconProxyUrl, item.FRId)}\"/>";

                            var post = PostData.CreateText(body, $"New Flash Sale: {item.Name}", tags);
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
                    ctx.FRItemFlashSales.Where(x => x.Item.FRId != itemId && x.RemovedAt == null).ToList().ForEach(x => x.RemovedAt = DateTime.UtcNow);
                    ctx.SaveChanges();
                }
            }
            await _serviceBus.CloseAsync();
        }
    }
}