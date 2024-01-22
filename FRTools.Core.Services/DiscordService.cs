using System.Net.Http.Headers;
using System.Text;
using FRTools.Core.Common.Extentions;
using FRTools.Core.Services.Discord.DiscordModels.MessageModels;
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
        private const string CHANNELWEBHOOKURL = "https://discord.com/api/v10/channels/{0}/webhooks";

        private readonly ILogger<DiscordService> _logger;

        public DiscordService(ILogger<DiscordService> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Message>> EditInitialInteraction(string token, DiscordWebhookRequest request) => await ProcessMessageToWebhook(request, CreateUrl(ORIGINALINTERACTIONURL, token), SendPatchRequest);
        public async Task<IEnumerable<Message>> EditInitialInteraction(string token, DiscordWebhookFiles request) => await ProcessFilesToWebhook(request, CreateUrl(ORIGINALINTERACTIONURL, token), SendPatchRequest);
        public async Task<IEnumerable<Message>> ReplyToInteraction(string token, DiscordWebhookRequest request) => await ProcessMessageToWebhook(request, CreateUrl(INTERACTIONURL, token), SendPostRequest);
        public async Task<IEnumerable<Message>> ReplyToInteraction(string token, DiscordWebhookFiles request) => await ProcessFilesToWebhook(request, CreateUrl(INTERACTIONURL, token), SendPostRequest);

        public async Task DeleteInteraction(string token)
        {
            using (var client = new HttpClient())
            {
                var url = CreateUrl(ORIGINALINTERACTIONURL, token);
                await client.DeleteAsync(url);
            }
        }

        public async Task<IEnumerable<Message>> PostMessageToWebhook(DiscordWebhookRequest webhook, string webhookUrl) => await ProcessMessageToWebhook(webhook, webhookUrl, SendPostRequest);

        public async Task<IEnumerable<Message>> PostFilesToWebhook(DiscordWebhookFiles webhook, string webhookUrl) => await ProcessFilesToWebhook(webhook, webhookUrl, SendPostRequest);

        public async Task<IEnumerable<Message>> PatchMessageToWebhook(DiscordWebhookRequest webhook, string webhookUrl) => await ProcessMessageToWebhook(webhook, webhookUrl, SendPatchRequest);

        public async Task<IEnumerable<Message>> PatchFilesToWebhook(DiscordWebhookFiles webhook, string webhookUrl) => await ProcessFilesToWebhook(webhook, webhookUrl, SendPatchRequest);

        public async Task<string> CreateWebhook(string name, ulong channelId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", Environment.GetEnvironmentVariable("DiscordBotToken"));
                var url = string.Format(CHANNELWEBHOOKURL, channelId);

                var response = await client.PostAsJsonAsync(url, new { name });
                var parsed = JsonConvert.DeserializeObject<DiscordWebhookResponse>(await response.Content.ReadAsStringAsync())!;

                return parsed.Url;
            }
        }

        private string CreateUrl(string url, string token) => string.Format(url, Environment.GetEnvironmentVariable("DiscordApplicationId"), token);

        private async Task<IEnumerable<Message>> ProcessMessageToWebhook(DiscordWebhookRequest webhook, string webhookUrl, Func<DiscordWebhookRequest, string, Task<Message?>> sendAction)
        {
            var messageResults = new List<Message>();

            // Webhooks can (only) have 10 embeds
            if (webhook.Embeds.Count() > 10)
            {
                foreach (var batch in webhook.Embeds.Select((e, i) => new { e, i }).GroupBy(x => x.i / 10))
                {
                    var webhookBatch = new DiscordWebhookRequest
                    {
                        Content = webhook.Content,
                        Username = webhook.Username,
                        AvatarUrl = webhook.AvatarUrl,
                        Embeds = batch.Select(x => x.e).ToArray()
                    };

                    var message = await sendAction(webhookBatch, webhookUrl);
                    if (message != null)
                        messageResults.Add(message);
                }
            }
            else
            {
                var message = await sendAction(webhook, webhookUrl);
                if (message != null)
                    messageResults.Add(message);
            }
            return messageResults;
        }

        private async Task<IEnumerable<Message>> ProcessFilesToWebhook(DiscordWebhookFiles webhook, string webhookUrl, Func<DiscordWebhookFiles, string, Task<Message?>> sendAction)
        {
            var messageResults = new List<Message>();

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
                            PayloadJson = new DiscordWebhookRequest
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
                                    PayloadJson = new DiscordWebhookRequest
                                    {
                                        Content = webhookBatch.PayloadJson.Content,
                                        Username = webhookBatch.PayloadJson.Username,
                                        AvatarUrl = webhookBatch.PayloadJson.AvatarUrl
                                    },
                                    Files = new Dictionary<string, byte[]>(fileBatch.Select(x => x.f).ToArray())
                                };

                                webhookFileBatch.PayloadJson.Embeds = webhookBatch.PayloadJson.Embeds.Where(x => webhookFileBatch.Files.Select(f => f.Key).Any(f => filesBelongingToEmbeds.Contains(f))).ToArray();

                                var message = await sendAction(webhookFileBatch, webhookUrl);
                                if (message != null)
                                    messageResults.Add(message);
                            }
                        }
                        else
                        {
                            var message = await sendAction(webhookBatch, webhookUrl);
                            if (message != null)
                                messageResults.Add(message);
                        }
                    }
                }
                else
                {
                    var message = await sendAction(webhook, webhookUrl);
                    if (message != null)
                        messageResults.Add(message);
                }
            }

            return messageResults;
        }

        private async Task<Message?> SendPatchRequest(DiscordWebhookRequest webhookObject, string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PatchAsJsonAsync(url, webhookObject);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error posting to webhook, response code: {0}\n\tUrl: {1}\n\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(webhookObject));
                    throw new HttpRequestException(null, null, response.StatusCode);
                }

                return JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync());
            }
        }

        private async Task<Message?> SendPatchRequest(DiscordWebhookFiles webhookObject, string url)
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
                        throw new HttpRequestException(null, null, response.StatusCode);
                    }

                    return JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync());
                }
            }
        }

        private async Task<Message?> SendPostRequest(DiscordWebhookRequest webhookObject, string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(url, webhookObject);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error posting to webhook, response code: {0}\n\tUrl: {1}\n\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(webhookObject));
                    throw new HttpRequestException(null, null, response.StatusCode);
                }

                return JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync());
            }
        }

        private async Task<Message?> SendPostRequest(DiscordWebhookFiles webhookObject, string url)
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
                        throw new HttpRequestException(null, null, response.StatusCode);
                    }

                    return JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync());
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
