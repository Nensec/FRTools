using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IDiscordService
    {
        Task DeleteInteraction(string token);
        Task EditInitialInteraction(string token, DiscordWebhook response);
        Task EditInitialInteraction(string token, DiscordWebhookFiles response);
        Task ReplyToInteraction(string token, DiscordWebhook response);
        Task ReplyToInteraction(string token, DiscordWebhookFiles response);
        Task PatchFilesToWebhook(DiscordWebhookFiles webhook, string webhookUrl);
        Task PatchMessageToWebhook(DiscordWebhook webhook, string webhookUrl);
        Task PostFilesToWebhook(DiscordWebhookFiles webhook, string webhookUrl);
        Task PostMessageToWebhook(DiscordWebhook webhook, string webhookUrl);
    }
}
