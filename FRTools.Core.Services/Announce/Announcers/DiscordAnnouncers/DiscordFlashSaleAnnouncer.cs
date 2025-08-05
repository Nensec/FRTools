using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Announce.Announcers.DiscordAnnouncers
{
    public interface IDiscordFlashSaleAnnouncer
    {
        Task AnnounceFlashSale(FlashSaleAnnounceData data);
    }

    public class DiscordFlashSaleAnnouncer : BaseDiscordAnnouncer, IDiscordFlashSaleAnnouncer
    {
        private readonly IItemAssetDataService _itemAssetDataService;
        private readonly IFRUserService _userService;
        private readonly ILogger<DiscordFlashSaleAnnouncer> _logger;

        public DiscordFlashSaleAnnouncer(IConfigService configService, IDiscordInteractionService discordInteractionService, IItemAssetDataService itemAssetDataService, IFRUserService userService, ILogger<DiscordFlashSaleAnnouncer> logger) : base(configService, discordInteractionService, logger)
        {
            _itemAssetDataService = itemAssetDataService;
            _userService = userService;
            _logger = logger;
        }

        public async Task AnnounceFlashSale(FlashSaleAnnounceData data)
        {
            var random = new Random();

            var webhook = new DiscordWebhookFilesRequest();
            var files = new Dictionary<string, byte[]>();
            var embeds = new List<DiscordEmbed>();

            var embed = new DiscordEmbed
            {
                Title = "New flash sale found! - " + data.FRItem.Name,
            };

            byte[]? itemAsset = null;
            try
            {
                itemAsset = await embed.ParseItemForEmbed(random, data.FRItem, _itemAssetDataService, _userService, _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to obtain item asset for item {data.FRItem.Id}");
            }

            embed.Fields.Add(new DiscordEmbedField { Inline = true, Name = "Marketplace", Value = $"[Click here]({data.MarketplaceLink})" });

            if (itemAsset != null)
            {
                var fileName = $"asset_{data.FRItem.FRId}.png";

                files.Add(fileName, itemAsset);
                embed.Image = new DiscordEmbedImage { Url = $"attachment://{fileName}" };

            }

            var iconAsset = await _itemAssetDataService.GetProxyIcon(data.FRItem.FRId);
            if (iconAsset != null)
                files.Add($"icon_{data.FRItem.FRId}.png", iconAsset);
            embeds.Add(embed);
            webhook.Files = files;
            webhook.PayloadJson.Embeds = embeds;
            var allFlashSaleWebhooks = (await ConfigService.GetAllConfig("GUILDCONFIG_FLASHSALEWEBHOOK")).Concat(await ConfigService.GetAllConfig("GUILDCONFIG_DEFAULTWEBHOOK")).GroupBy(x => x.GuildId);

            foreach (var guildWebhooks in allFlashSaleWebhooks)
            {
                await AttemptPostToWebhook(guildWebhooks, webhook);
            }
        }

    }
}
