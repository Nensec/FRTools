﻿using FRTools.Core.Services.Discord.DiscordModels.MessageModels;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;

namespace FRTools.Core.Services.Interfaces
{
    public interface IDiscordService
    {
        Task DeleteInteraction(string token);
        Task<IEnumerable<Message>> EditInitialInteraction(string token, DiscordWebhook response);
        Task<IEnumerable<Message>> EditInitialInteraction(string token, DiscordWebhookFiles response);
        Task<IEnumerable<Message>> ReplyToInteraction(string token, DiscordWebhook response);
        Task<IEnumerable<Message>> ReplyToInteraction(string token, DiscordWebhookFiles response);
        Task<IEnumerable<Message>> PatchFilesToWebhook(DiscordWebhookFiles webhook, string webhookUrl);
        Task<IEnumerable<Message>> PatchMessageToWebhook(DiscordWebhook webhook, string webhookUrl);
        Task<IEnumerable<Message>> PostFilesToWebhook(DiscordWebhookFiles webhook, string webhookUrl);
        Task<IEnumerable<Message>> PostMessageToWebhook(DiscordWebhook webhook, string webhookUrl);
    }
}