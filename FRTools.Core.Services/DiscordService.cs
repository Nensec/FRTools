using System.Net.Http.Headers;
using System.Text;
using FRTools.Core.Common.Extentions;
using FRTools.Core.Services.Discord.DiscordModels.WebhookModels;
using FRTools.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FRTools.Core.Services
{
    public class DiscordService : IDiscordService
    {
        private const string INTERACTIONURL = "https://discord.com/api/v10/webhooks/{0}/{1}";
        private const string ORIGINALINTERACTIONURL = "https://discord.com/api/v10/webhooks/{0}/{1}/messages/@original";

        private readonly ILogger<DiscordService> _logger;

        public DiscordService(ILogger<DiscordService> logger)
        {
            _logger = logger;
        }

        public async Task EditInitialInteraction(string token, DiscordWebhook response) => await ProcessMessageToWebhook(response, CreateUrl(ORIGINALINTERACTIONURL, token), SendPatchRequest);
        public async Task EditInitialInteraction(string token, DiscordWebhookFiles response) => await ProcessFilesToWebhook(response, CreateUrl(ORIGINALINTERACTIONURL, token), SendPatchRequest);
        public async Task ReplyToInteraction(string token, DiscordWebhook response) => await ProcessMessageToWebhook(response, CreateUrl(INTERACTIONURL, token), SendPostRequest);
        public async Task ReplyToInteraction(string token, DiscordWebhookFiles response) => await ProcessFilesToWebhook(response, CreateUrl(INTERACTIONURL, token), SendPostRequest);

        public async Task DeleteInteraction(string token)
        {
            using (var client = new HttpClient())
            {
                var url = CreateUrl(ORIGINALINTERACTIONURL, token);
                await client.DeleteAsync(url);
            }
        }

        public async Task PostMessageToWebhook(DiscordWebhook webhook, string webhookUrl) => await ProcessMessageToWebhook(webhook, webhookUrl, SendPostRequest);

        public async Task PostFilesToWebhook(DiscordWebhookFiles webhook, string webhookUrl) => await ProcessFilesToWebhook(webhook, webhookUrl, SendPostRequest);

        public async Task PatchMessageToWebhook(DiscordWebhook webhook, string webhookUrl) => await ProcessMessageToWebhook(webhook, webhookUrl, SendPatchRequest);

        public async Task PatchFilesToWebhook(DiscordWebhookFiles webhook, string webhookUrl) => await ProcessFilesToWebhook(webhook, webhookUrl, SendPatchRequest);

        private string CreateUrl(string url, string token) => string.Format(url, Environment.GetEnvironmentVariable("DiscordApplicationId"), token);

        private async Task ProcessMessageToWebhook(DiscordWebhook webhook, string webhookUrl, Func<DiscordWebhook, string, Task> sendAction)
        {
            // Webhooks can (only) have 10 embeds
            if (webhook.Embeds.Count() > 10)
            {
                foreach (var batch in webhook.Embeds.Select((e, i) => new { e, i }).GroupBy(x => x.i / 10))
                {
                    var webhookBatch = new DiscordWebhook
                    {
                        Content = webhook.Content,
                        Username = webhook.Username,
                        AvatarUrl = webhook.AvatarUrl,
                        Embeds = batch.Select(x => x.e).ToArray()
                    };

                    await sendAction(webhookBatch, webhookUrl);
                }
            }
            else
                await sendAction(webhook, webhookUrl);
        }

        private async Task ProcessFilesToWebhook(DiscordWebhookFiles webhook, string webhookUrl, Func<DiscordWebhookFiles, string, Task> sendAction)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                // Webhooks can (only) have 10 embeds
                if (webhook.PayloadJson.Embeds.Count() > 10)
                {
                    foreach (var batch in webhook.PayloadJson.Embeds.Select((e, i) => new { e, i }).GroupBy(x => x.i / 10))
                    {
                        var webhookBatch = new DiscordWebhookFiles
                        {
                            PayloadJson = new DiscordWebhook
                            {
                                Content = webhook.PayloadJson.Content,
                                Username = webhook.PayloadJson.Username,
                                AvatarUrl = webhook.PayloadJson.AvatarUrl,
                                Embeds = batch.Select(x => x.e).ToArray()
                            }
                        };

                        var filesBelongingToEmbeds = webhookBatch.PayloadJson.Embeds.Where(x => x.Image?.Url.StartsWith("attachment://") == true).Select(x => x.Image.Url.Replace("attachment://", "")).ToArray()
                            .Concat(webhookBatch.PayloadJson.Embeds.Where(x => x.Auhor?.IconUrl.StartsWith("attachment://") == true).Select(x => x.Auhor.IconUrl.Replace("attachment://", "")).ToArray())
                            .Concat(webhookBatch.PayloadJson.Embeds.Where(x => x.Thumbnail?.Url.StartsWith("attachment://") == true).Select(x => x.Thumbnail.Url.Replace("attachment://", "")).ToArray())
                            .Distinct();

                        webhookBatch.Files = new Dictionary<string, byte[]>(webhook.Files.Where(x => filesBelongingToEmbeds.Contains(x.Key)).ToArray());

                        // Max 10 files as well
                        if (webhookBatch.Files.Count() > 10)
                        {
                            foreach (var fileBatch in webhookBatch.Files.Select((f, i) => new { f, i }).GroupBy(x => x.i / 10))
                            {
                                var webhookFileBatch = new DiscordWebhookFiles
                                {
                                    PayloadJson = new DiscordWebhook
                                    {
                                        Content = webhookBatch.PayloadJson.Content,
                                        Username = webhookBatch.PayloadJson.Username,
                                        AvatarUrl = webhookBatch.PayloadJson.AvatarUrl
                                    },
                                    Files = new Dictionary<string, byte[]>(fileBatch.Select(x => x.f).ToArray())
                                };

                                webhookFileBatch.PayloadJson.Embeds = webhookBatch.PayloadJson.Embeds.Where(x => webhookFileBatch.Files.Select(f => f.Key).Any(f => filesBelongingToEmbeds.Contains(f))).ToArray();

                                await sendAction(webhookFileBatch, webhookUrl);
                            }
                        }
                        else
                            await sendAction(webhookBatch, webhookUrl);
                    }
                }
                else
                    await sendAction(webhook, webhookUrl);
            }
        }

        private async Task SendPatchRequest(DiscordWebhook webhookObject, string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PatchAsJsonAsync(url, webhookObject);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error posting to webhook, response code: {0}\n\tUrl: {1}\n\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(webhookObject));
                }
            }
        }

        private async Task SendPatchRequest(DiscordWebhookFiles webhookObject, string url)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("--boundary"))
                {
                    content.Add(new StringContent(JsonConvert.SerializeObject(webhookObject.PayloadJson), Encoding.UTF8, "application/json"), "payload_json");
                    foreach (var file in webhookObject.Files.Select((Data, Index) => (Index, Data)))
                        content.Add(CreateFileContent(file.Data.Value, "image/png"), $"files[{file.Index}]", file.Data.Key);

                    var response = await client.PatchAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Error posting to webhook, response code: {0}\n\tUrl: {1}\n\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(webhookObject));
                    }
                }
            }
        }

        private async Task SendPostRequest(DiscordWebhook webhookObject, string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(url, webhookObject);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error posting to webhook, response code: {0}\n\tUrl: {1}\n\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(webhookObject));
                }
            }
        }

        private async Task SendPostRequest(DiscordWebhookFiles webhookObject, string url)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("--boundary"))
                {
                    content.Add(new StringContent(JsonConvert.SerializeObject(webhookObject.PayloadJson), Encoding.UTF8, "application/json"), "payload_json");
                    foreach (var file in webhookObject.Files.Select((Data, Index) => (Index, Data)))
                        content.Add(CreateFileContent(file.Data.Value, "image/png"), $"files[{file.Index}]", file.Data.Key);

                    var response = await client.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Error posting to webhook, response code: {0}\n\tUrl: {1}\n\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(webhookObject));
                    }
                }
            }
        }

        private ByteArrayContent CreateFileContent(byte[] data, string contentType)
        {
            var fileContent = new ByteArrayContent(data);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }
    }
}
