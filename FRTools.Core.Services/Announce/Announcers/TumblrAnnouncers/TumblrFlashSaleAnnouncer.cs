using System.Text.RegularExpressions;
using DontPanic.TumblrSharp;
using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Announce.Announcers.TumblrAnnouncers
{
    public interface ITumblrFlashSaleAnnouncer
    {
        Task AnnounceFlashSale(FlashSaleAnnounceData data);
    }

    public class TumblrFlashSaleAnnouncer : ITumblrFlashSaleAnnouncer
    {
        private readonly IFRUserService _userService;
        private readonly ITumblrService _tumblrService;
        private readonly ILogger<TumblrFlashSaleAnnouncer> _logger;

        public TumblrFlashSaleAnnouncer(IFRUserService userService, ITumblrService tumblrService, ILogger<TumblrFlashSaleAnnouncer> logger)
        {
            _userService = userService;
            _tumblrService = tumblrService;
            _logger = logger;
        }
        public async Task AnnounceFlashSale(FlashSaleAnnounceData data)
        {
            var tags = new List<string> { "frtools", "fr tools", "flight rising", "flightrising", "fr", "flash sale", "flashsale", data.FRItem.Name.ToLower() };
            string body = $"<p>A new flash sale has been discovered for <b>{data.FRItem.Name}</b></p>";
            body += $"<p><i>{data.FRItem.Description}</i></p><br/>";
            body += "<p>";
            body += $"Game database: <a href=\"{string.Format(FRHelpers.GameDatabaseUrl, data.FRItem.FRId)}\">click here</a><br/>";
            body += $"Marketplace link: <a href=\"{data.MarketplaceLink}\">click here</a><br/>";
            body += "</p>";

            if (data.FRItem.TreasureValue > 0)
                body += $"Treasure: <strike>{data.FRItem.TreasureValue * 10}</strike> <b>{data.FRItem.TreasureValue * .8 * 10}</b><br/>";

            if (data.FRItem.FoodValue > 0)
                body += $"Food: {data.FRItem.FoodValue}<br/>";

            string? itemUrl = null;
            var random = new Random();
            switch (data.FRItem.ItemCategory)
            {
                case FRItemCategory.Skins:
                    var skinType = data.FRItem.ItemType.Split(' ');
                    var dragonType = (DragonType)Enum.Parse(typeof(DragonType), skinType[0]);
                    if (Enum.TryParse<Gender>(skinType[1], out var gender))
                        itemUrl = Helpers.GetProxyDummyDragonSkinUrl(dragonType, gender, data.FRItem.Id);

                    body += $"For: {dragonType} {gender}<br/>";

                    var username = Regex.Match(data.FRItem.Description, @"Designed by ([^.]+)[.|\)]");
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
                        itemUrl = Helpers.GetProxyDummyDragonApparelUrl(modernBreeds[random.Next(1, modernBreeds.Length)], random.Next(0, 2), data.FRItem.FRId);
                        tags.Add(data.FRItem.ItemType.ToLower());
                        break;
                    }
                case FRItemCategory.Familiar:
                    itemUrl = string.Format(FRHelpers.FamiliarArtUrl, data.FRItem.FRId);
                    tags.Add("fr familiar");
                    tags.Add("familiar");
                    break;
                case FRItemCategory.Trinket when data.FRItem.ItemType == "Specialty Item" && (data.FRItem.Name.StartsWith("Primary") || data.FRItem.Name.StartsWith("Secondary") || data.FRItem.Name.StartsWith("Tertiary")):
                    tags.Add($"{(data.FRItem.Name.StartsWith("Primary") ? "primary" : data.FRItem.Name.StartsWith("Secondary") ? "secondary" : "tertiarty")} gene");
                    tags.Add("gene");
                    tags.Add(data.FRItem.Name.Split(':')[1].ToLower());

                    if (FRHelpers.IsAncientGene(data.FRItem, out var ancientBreed))
                    {
                        itemUrl = Helpers.GetProxyDummyDragonGeneUrl(ancientBreed!.Value, random.Next(0, 2), data.FRItem.FRId);
                        tags.Add("ancient gene");
                        tags.Add(ancientBreed.Value.ToString().ToLower());
                    }
                    else
                    {
                        var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                        itemUrl = Helpers.GetProxyDummyDragonGeneUrl((int)modernBreeds[random.Next(1, modernBreeds.Length)], random.Next(0, 2), data.FRItem.FRId);
                    }
                    break;
                case FRItemCategory.Trinket when data.FRItem.ItemType == "Scene":
                    itemUrl = string.Format(FRHelpers.SceneArtUrl, data.FRItem.FRId);
                    tags.Add("scene");
                    break;
                default:
                    if (data.FRItem.AssetUrl != null)
                    {
                        _logger.LogWarning("Unknown type for item {0}", data.FRItem.FRId);
                        if (data.FRItem.AssetUrl != null)
                            itemUrl = $"https://www1.flightrising.com{data.FRItem.AssetUrl}";
                        tags.Add(data.FRItem.ItemType.ToLower());
                    }
                    break;
            }

            body += $"<img src=\"{itemUrl ?? Helpers.GetProxyIconUrl(data.FRItem.FRId)}";

            var post = PostData.CreateText(body, $"New Flash Sale: {data.FRItem.Name}", tags);

            await _tumblrService.CreatePost("frtools", post);
        }

    }
}
