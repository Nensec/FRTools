using System.Text.RegularExpressions;
using DontPanic.TumblrSharp;
using FRTools.Core.Common;
using FRTools.Core.Data;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Announce.Announcers
{
    public class TumblrAnnouncer : IDominanceAnnouncer, IFlashSaleAnnouncer
    {
        private readonly IFRUserService _userService;
        private readonly ITumblrService _tumblrService;
        private readonly ILogger<TumblrAnnouncer> _logger;

        public TumblrAnnouncer(IFRUserService userService, ITumblrService tumblrService, ILogger<TumblrAnnouncer> logger)
        {
            _userService = userService;
            _tumblrService = tumblrService;
            _logger = logger;
        }

        public async Task Announce(AnnounceData announceData)
        {
            switch (announceData)
            {
                case DominanceAnnounceData dominanceAnnounceData:
                    await AnnounceDominance(dominanceAnnounceData);
                    break;
                case FlashSaleAnnounceData flashSaleAnnounceData:
                    await AnnounceFlashSale(flashSaleAnnounceData);
                    break;
            }
        }

        private async Task AnnounceDominance(DominanceAnnounceData data)
        {
            var body = $"<p>Dominance has been calculated and the winner of this week is <b>{data.Flights[0]}</b>!</p>";
            body += "<p>The top 3 standings were as follows:";
            body += "<ol>";
            body += $"<li>{data.Flights[0]}</li>";
            body += $"<li>{data.Flights[1]}</li>";
            body += $"<li>{data.Flights[2]}</li>";
            body += "</ol></p>";

            if (data.Flights[0] != Flight.Beastclans)
            {
                body += "<p>First place gets a nice 15% discount on the treasure market place and a 5% discount on lair upgrades. Additionally, they get +1500 treasure a day and +3 gathering turns.</p>";
            }
            else
            {
                body += "<p>Wait.. Beastclans won!? Why did Earth not win at least..? Alright.. well, instead of first place we'll just list the second place benefits then I suppose..<br/>";
                body += "Second place gets a nice 7% discount on the treasure market place and a 1% discount on lair upgrades..<br/>";
                body += "They also get +750 treasure a day and +2 gathering turns..</p>";
            }

            var tags = new List<string> { "frtools", "fr tools", "flight rising", "flightrising", "fr", "dominance", data.Flights[0].ToString(), data.Flights[1].ToString(), data.Flights[2].ToString() };

            var post = PostData.CreateText(body, $"Congratulations to {data.Flights[0]}!", tags);
#if DEBUG
            post.State = PostCreationState.Private;
#endif

            await _tumblrService.CreatePost("frtools", post);
        }

        private async Task AnnounceFlashSale(FlashSaleAnnounceData data)
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
                    var gender = (Gender)Enum.Parse(typeof(Gender), skinType[1]);
                    itemUrl = Common.Helpers.GetProxyDummyDragonSkinUrl(dragonType, gender, data.FRItem.Id);
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
                        itemUrl = Common.Helpers.GetProxyDummyDragonApparelUrl(modernBreeds[random.Next(1, modernBreeds.Length)], random.Next(0, 2), data.FRItem.FRId);
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
                    var ancientBreed = GeneratedFRHelpers.GetAncientBreeds().Cast<DragonType?>().FirstOrDefault(x => data.FRItem.Name.EndsWith($"({x})"));
                    if (ancientBreed.HasValue)
                    {
                        itemUrl = Common.Helpers.GetProxyDummyDragonGeneUrl(ancientBreed.Value, random.Next(0, 2), data.FRItem.FRId);
                        tags.Add("ancient gene");
                        tags.Add(ancientBreed.Value.ToString().ToLower());
                    }
                    else
                    {
                        var modernBreeds = GeneratedFRHelpers.GetModernBreeds();
                        itemUrl = Common.Helpers.GetProxyDummyDragonGeneUrl((int)modernBreeds[random.Next(1, modernBreeds.Length)], random.Next(0, 2), data.FRItem.FRId);
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

            body += $"<img src=\"{itemUrl ?? Common.Helpers.GetProxyIconUrl(data.FRItem.FRId)}";

            var post = PostData.CreateText(body, $"New Flash Sale: {data.FRItem.Name}", tags);
#if DEBUG
            post.State = PostCreationState.Private;
#endif
            await _tumblrService.CreatePost("frtools", post);
        }
    }
}
