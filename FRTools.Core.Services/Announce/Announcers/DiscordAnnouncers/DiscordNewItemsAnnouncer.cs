using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Announce.Announcers.DiscordAnnouncers
{
    public interface IDiscordNewItemsAnnouncer
    {
        Task AnnounceNewItems(NewItemsAnnounceData data);
    }

    public class DiscordNewItemsAnnouncer : BaseDiscordAnnouncer, IDiscordNewItemsAnnouncer
    {
        private readonly IItemAssetDataService _itemAssetDataService;
        private readonly IFRUserService _userService;
        private readonly ILogger<DiscordNewItemsAnnouncer> _logger;

        public DiscordNewItemsAnnouncer(IConfigService configService, IDiscordInteractionService discordInteractionService, IItemAssetDataService itemAssetDataService, IFRUserService userService, ILogger<DiscordNewItemsAnnouncer> logger) : base(configService, discordInteractionService, logger)
        {
            _itemAssetDataService = itemAssetDataService;
            _userService = userService;
            _logger = logger;
        }

        public async Task AnnounceNewItems(NewItemsAnnounceData data)
        {
            var random = new Random();

            var webhook = new DiscordWebhookFilesRequest();
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

                byte[]? itemAsset = null;
                try
                {
                    itemAsset = await embed.ParseItemForEmbed(random, item, _itemAssetDataService, _userService, _logger);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unable to obtain item asset for item {item.Id}");
                }

                if (itemAsset != null)
                {
                    var fileName = $"asset_{item.FRId}.png";

                    files.Add(fileName, itemAsset);
                    embed.Image = new DiscordEmbedImage { Url = $"attachment://{fileName}" };

                }

                var iconAsset = await _itemAssetDataService.GetProxyIcon(item.FRId);
                if (iconAsset != null)
                    files.Add($"icon_{item.FRId}.png", iconAsset);

                itemEmbeds.Add(item, (embed, files));
            }

            embeds.AddRange(itemEmbeds.Values.Select(x => x.Embed));
            foreach (var files in itemEmbeds.Values.SelectMany(x => x.Files))
                webhook.Files.Add(files.Key, files.Value);
            webhook.PayloadJson.Embeds = embeds;

            var allNewItemWebhooks = (await ConfigService.GetAllConfig("GUILDCONFIG_NEWITEMSWEBHOOK")).Concat(await ConfigService.GetAllConfig("GUILDCONFIG_DEFAULTWEBHOOK")).GroupBy(x => x.GuildId);

            foreach (var guildWebhooks in allNewItemWebhooks)
            {
                var webhookToPost = webhook;

                var guildSpecificSettings = await ConfigService.GetConfigValue("GUILDCONFIG_ANNOUNCE_NEWITEMTYPES", guildWebhooks.Key);
                if (guildSpecificSettings != null)
                {
                    webhookToPost = new DiscordWebhookFilesRequest();
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

    }
}
