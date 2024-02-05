using FRTools.Core.Common;
using FRTools.Core.Data.DataModels.FlightRisingModels;
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

            var allDominanceWebhooks = (await _configService.GetAllConfig("GUILDCONFIG_DOMINANCEWEBHOOK")).Concat(await _configService.GetAllConfig("GUILDCONFIG_DEFAULTWEBHOOK")).GroupBy(x => x.GuildId);

            foreach (var guildWebhooks in allDominanceWebhooks)
            {
                await AttemptPostToWebhook(guildWebhooks, webhook);
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

            var allFlashSaleWebhooks = (await _configService.GetAllConfig("GUILDCONFIG_FLASHSALEWEBHOOK")).Concat(await _configService.GetAllConfig("GUILDCONFIG_DEFAULTWEBHOOK")).GroupBy(x => x.GuildId);

            foreach (var guildWebhooks in allFlashSaleWebhooks)
            {
                await AttemptPostToWebhook(guildWebhooks, webhook);
            }
        }

        private async Task AnnounceNewItems(NewItemsAnnounceData data)
        {
            var random = new Random();

            var webhook = new DiscordWebhookFiles();
            var embeds = new List<DiscordEmbed>();

            var itemEmbeds = new Dictionary<FRItem, (DiscordEmbed Embed, Dictionary<string, byte[]> Files)>();

            foreach (var item in data.FRItems)
            {
                var files = new Dictionary<string, byte[]>();
                var embed = new DiscordEmbed
                {
                    Title = "New item found! - " + item.Name,
                    Description = item.Description,
                    Thumbnail = new DiscordEmbedThumbnail { Url = $"attachment://icon_{item.FRId}.png" }
                };

                var fields = new List<DiscordEmbedField>();

                if (item.TreasureValue > 0)
                    fields.Add(new DiscordEmbedField { Name = "Treasure value", Value = $"{item.TreasureValue}", Inline = true });

                embed.Fields = fields;

                byte[]? itemAsset = await embed.ParseItemForEmbed(random, item, _userService, _logger);

                if (itemAsset != null)
                {
                    var fileName = $"asset_{item.FRId}.png";

                    files.Add(fileName, itemAsset);
                    embed.Image = new DiscordEmbedImage { Url = $"attachment://{fileName}" };

                }
                using (var client = new HttpClient())
                {
                    var iconAsset = await client.GetByteArrayAsync(Helpers.GetProxyIconUrl(item.FRId));
                    files.Add($"icon_{item.FRId}.png", iconAsset);
                }

                itemEmbeds.Add(item, (embed, files));
            }

            embeds.AddRange(itemEmbeds.Values.Select(x => x.Embed));
            foreach (var files in itemEmbeds.Values.SelectMany(x => x.Files))
                webhook.Files.Add(files.Key, files.Value);
            webhook.PayloadJson.Embeds = embeds;

            var allNewItemWebhooks = (await _configService.GetAllConfig("GUILDCONFIG_NEWITEMSWEBHOOK")).Concat(await _configService.GetAllConfig("GUILDCONFIG_DEFAULTWEBHOOK")).GroupBy(x => x.GuildId);

            foreach (var guildWebhooks in allNewItemWebhooks)
            {
                var webhookToPost = webhook;

                var guildSpecificSettings = await _configService.GetConfigValue("GUILDCONFIG_ANNOUNCE_NEWITEMTYPES", guildWebhooks.Key);
                if (guildSpecificSettings != null)
                {
                    webhookToPost = new DiscordWebhookFiles();
                    var itemCategoriesAllowed = guildSpecificSettings.Split(',').Select(x => Enum.Parse<FRItemCategory>(x)).ToList();

                    foreach (var itemEmbed in itemEmbeds)
                    {
                        if (itemCategoriesAllowed.Contains(itemEmbed.Key.ItemCategory))
                        {
                            webhookToPost.PayloadJson.Embeds.Add(itemEmbed.Value.Embed);
                            foreach (var files in itemEmbed.Value.Files)
                                webhook.Files.Add(files.Key, files.Value);
                        }
                    }
                }

                await AttemptPostToWebhook(guildWebhooks, webhookToPost);
            }
        }

        private async Task AttemptPostToWebhook(IGrouping<ulong, (string Key, string Value, ulong GuildId)> guildWebhooks, DiscordWebhookFiles webhook)
        {
            foreach (var guildWebhook in guildWebhooks)
            {
                try
                {
                    await _discordService.PostFilesToWebhook(webhook, guildWebhook.Value);
                    break;
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized || ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning(ex, "Unable to post to webhook, but got unauthorized or not found. Webhook access or webhook removed, deleting record and attempting possible next registered webhook.");
                    await _configService.RemoveConfig(guildWebhook.Key, guildWebhook.GuildId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to post to webhook.");
                }
            }            
        }

        private async Task AttemptPostToWebhook(IGrouping<ulong, (string Key, string Value, ulong GuildId)> guildWebhooks, DiscordWebhookRequest webhook)
        {
            foreach (var guildWebhook in guildWebhooks)
            {
                try
                {
                    await _discordService.PostMessageToWebhook(webhook, guildWebhook.Value);
                    break;
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning(ex, "Unable to post to webhook, but got unauthorized. Webhook access removed, deleting record and attempting possible next registered webhook.");
                    await _configService.RemoveConfig(guildWebhook.Key, guildWebhook.GuildId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to post to webhook.");
                }
            }
        }
    }
}
