using FRTools.Core.Common;
using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Announce.Announcers
{
    public class DiscordWebhookAnnouncer : IFlashSaleAnnouncer, IDominanceAnnouncer, INewItemAnnouncer
    {
        private readonly IConfigService _configService;
        private readonly IDiscordService _discordService;
        private readonly IFRUserService _userService;
        private readonly ILogger<DiscordWebhookAnnouncer> _logger;

        public DiscordWebhookAnnouncer(IConfigService configService, IDiscordService discordService, IFRUserService userService, ILogger<DiscordWebhookAnnouncer> logger)
        {
            _configService = configService;
            _discordService = discordService;
            _userService = userService;
            _logger = logger;
        }

        public async Task Announce(AnnounceData announceData)
        {
            switch (announceData)
            {
                case FlashSaleAnnounceData flashSaleData:
                    await AnnounceFlashSale(flashSaleData);
                    break;
                case DominanceAnnounceData dominanceAnnounceData:
                    await AnnounceDominance(dominanceAnnounceData);
                    break;
                case NewItemsAnnounceData newItemsData:
                    await AnnounceNewItems(newItemsData);
                    break;
            }
        }

        private async Task AnnounceDominance(DominanceAnnounceData dominanceAnnounceData)
        {
            var webhook = new DiscordWebhookRequest();
            var embed = new DiscordEmbed
            {
                Title = $"Congratulations to the **{dominanceAnnounceData.Flights[0]}** flight for claiming 1st place!",
                Fields = new List<DiscordEmbedField>
                {
                    new() { Name = "Info", Value = $"Those who are part of the {dominanceAnnounceData.Flights[0]} flight, enjoy your discounts!\r\n\nSecond place: {dominanceAnnounceData.Flights[1]}\r\nThird place: {dominanceAnnounceData.Flights[2]}"},
                    new() { Name = "Benefits", Value = $"15% off marketplace treasure items\r\n5% off lair expansions\r\n+1500 treasure a day\r\n+3 gathering turns a day"}
                }
            };
            webhook.Embeds = new List<DiscordEmbed> { embed };

            var dominanceWebhooks = (await _configService.GetAllConfig("GUILDCONFIG_DOMINANCEWEBHOOK")).Concat(await _configService.GetAllConfig("GUILDCONFIG_DEFAULTWEBHOOK"));

            foreach (var dominanceWebhook in dominanceWebhooks.GroupBy(x => x.GuildId).First())
            {
                try
                {
                    await _discordService.PostMessageToWebhook(webhook, dominanceWebhook.Value);
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _configService.RemoveConfig("GUILDCONFIG_DOMINANCEWEBHOOK", dominanceWebhook.GuildId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to post to webhook.");
                }
            }
        }

        private async Task AnnounceFlashSale(FlashSaleAnnounceData data)
        {
            var random = new Random();

            var webhook = new DiscordWebhookFiles();
            var files = new Dictionary<string, byte[]>();
            var embeds = new List<DiscordEmbed>();

            var embed = new DiscordEmbed
            {
                Title = "New flash sale found! - " + data.FRItem.Name,
            };
            var fields = new List<DiscordEmbedField>
            {
                new() {
                    Name = "Game database",
                    Value = $"[#{data.FRItem.FRId}]({string.Format(FRHelpers.GameDatabaseUrl, data.FRItem.FRId)})",
                    Inline = true
                }
            };

            if (data.FRItem.TreasureValue > 0)
                fields.Add(new DiscordEmbedField { Name = "Treasure value", Value = $"~~{data.FRItem.TreasureValue * 10}~~ ***{data.FRItem.TreasureValue * .8 * 10}***", Inline = true });

            byte[]? itemAsset = await embed.ParseItemForEmbed(random, data.FRItem, _userService, _logger);

            embed.Fields = fields;

            if (itemAsset != null)
            {
                var fileName = $"asset_{data.FRItem.FRId}.png";

                files.Add(fileName, itemAsset);
                embed.Image = new DiscordEmbedImage { Url = $"attachment://{fileName}" };

            }
            using (var client = new HttpClient())
            {
                var iconAsset = await client.GetByteArrayAsync(Helpers.GetProxyIconUrl(data.FRItem.FRId));
                files.Add($"icon_{data.FRItem.FRId}.png", iconAsset);
            }
            embeds.Add(embed);

            webhook.Files = files;
            webhook.PayloadJson.Embeds = embeds;

            var flashSaleWebhooks = (await _configService.GetAllConfig("GUILDCONFIG_FLASHSALEWEBHOOK")).Concat(await _configService.GetAllConfig("GUILDCONFIG_DEFAULTWEBHOOK"));

            foreach (var flashSaleWebhook in flashSaleWebhooks.GroupBy(x => x.GuildId).First())
            {
                try
                {
                    await _discordService.PostFilesToWebhook(webhook, flashSaleWebhook.Value);
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _configService.RemoveConfig("GUILDCONFIG_FLASHSALEWEBHOOK", flashSaleWebhook.GuildId);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to post to webhook.");
                }
            }
        }

        private async Task AnnounceNewItems(NewItemsAnnounceData data)
        {
            var random = new Random();

            var webhook = new DiscordWebhookFiles();
            var embeds = new List<DiscordEmbed>();

            foreach (var item in data.FRItems)
            {
                var embed = new DiscordEmbed
                {
                    Title = item.Name,
                    Description = item.Description,
                    Thumbnail = new DiscordEmbedThumbnail { Url = $"attachment://icon_{item.FRId}.png" }
                };
                var fields = new List<DiscordEmbedField>
                {
                    new() {
                        Name = "Game database",
                        Value = $"[#{item.FRId}]({string.Format(FRHelpers.GameDatabaseUrl, item.FRId)})",
                        Inline = true
                    }
                };

                if (item.TreasureValue > 0)
                    fields.Add(new DiscordEmbedField { Name = "Treasure value", Value = $"{item.TreasureValue}", Inline = true });

                embed.Fields = fields;

                byte[]? itemAsset = await embed.ParseItemForEmbed(random, item, _userService, _logger);

                if (itemAsset != null)
                {
                    var fileName = $"asset_{item.FRId}.png";

                    webhook.Files.Add(fileName, itemAsset);
                    embed.Image = new DiscordEmbedImage { Url = $"attachment://{fileName}" };

                }
                using (var client = new HttpClient())
                {
                    var iconAsset = await client.GetByteArrayAsync(Helpers.GetProxyIconUrl(item.FRId));
                    webhook.Files.Add($"icon_{item.FRId}.png", iconAsset);
                }
                embeds.Add(embed);
            }

            webhook.PayloadJson.Embeds = embeds;

            var newItemseWebhooks = (await _configService.GetAllConfig("GUILDCONFIG_NEWITEMSWEBHOOK")).Concat(await _configService.GetAllConfig("GUILDCONFIG_DEFAULTWEBHOOK"));

            foreach (var newItemseWebhook in newItemseWebhooks.GroupBy(x => x.GuildId).First())
            {
                try
                {
                    await _discordService.PostFilesToWebhook(webhook, newItemseWebhook.Value);
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _configService.RemoveConfig("GUILDCONFIG_NEWITEMSWEBHOOK", newItemseWebhook.GuildId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to post to webhook.");
                }
            }
        }
    }
}
