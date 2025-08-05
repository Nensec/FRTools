using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FRTools.Core.Services.Announce.Announcers.DiscordAnnouncers
{
    public abstract class BaseDiscordAnnouncer
    {
        protected IConfigService ConfigService { get; }
        protected IDiscordInteractionService DiscordInteractionService { get; }
        private readonly ILogger _logger;

        protected BaseDiscordAnnouncer(IConfigService configService, IDiscordInteractionService discordInteractionService, ILogger logger)
        {
            ConfigService = configService;
            DiscordInteractionService = discordInteractionService;
            _logger = logger;
        }

        protected async Task AttemptPostToWebhook(IGrouping<ulong, (string Key, string Value, ulong GuildId)> guildWebhooks, IDiscordWebhookRequest webhookRequest)
        {
#if DEBUG
            guildWebhooks = new[] { (string.Empty, Environment.GetEnvironmentVariable("DebugWebhook")!, (ulong)0) }.GroupBy(x => x.Item3).First();
#endif
            foreach (var guildWebhook in guildWebhooks)
            {
                try
                {
                    await DiscordInteractionService.PostMessageToWebhook(webhookRequest, guildWebhook.Value);
                    break;
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning(ex, "Unable to post to webhook, but got unauthorized. Webhook access removed, deleting record and attempting possible next registered webhook.");
                    await ConfigService.RemoveConfig(guildWebhook.Key, guildWebhook.GuildId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to post to webhook.");
                }
            }
        }

    }
}
