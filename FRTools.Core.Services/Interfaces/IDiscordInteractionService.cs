using FRTools.Core.Services.Discord.DiscordModels.MessageModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IDiscordInteractionService
    {
        Task DeleteInteraction(string token);
        Task<IEnumerable<Message>> EditInitialInteraction(string token, IDiscordWebhookRequest response);
        Task<IEnumerable<Message>> ReplyToInteraction(string token, IDiscordWebhookRequest response);
        Task<IEnumerable<Message>> PatchMessageToWebhook(IDiscordWebhookRequest webhookRequest, string webhookUrl);
        Task<IEnumerable<Message>> PostMessageToWebhook(IDiscordWebhookRequest webhookRequest, string webhookUrl);
        Task<string> CreateWebhook(string name, ulong channelId);
    }
}
