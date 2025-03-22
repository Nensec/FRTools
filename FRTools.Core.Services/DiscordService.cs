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

        public async Task<IEnumerable<Message>> EditInitialInteraction(string token, IDiscordWebhookRequest webhookRequest) => await ProcessRequest(webhookRequest, CreateUrl(ORIGINALINTERACTIONURL, token), SendPatchRequest);
        public async Task<IEnumerable<Message>> ReplyToInteraction(string token, IDiscordWebhookRequest webhookRequest) => await ProcessRequest(webhookRequest, CreateUrl(INTERACTIONURL, token), SendPostRequest);
        public async Task<IEnumerable<Message>> PostMessageToWebhook(IDiscordWebhookRequest webhookRequest, string webhookUrl) => await ProcessRequest(webhookRequest, webhookUrl, SendPostRequest);
        public async Task<IEnumerable<Message>> PatchMessageToWebhook(IDiscordWebhookRequest webhookRequest, string webhookUrl) => await ProcessRequest(webhookRequest, webhookUrl, SendPatchRequest);
        public async Task DeleteInteraction(string token)
        {
            using (var client = new HttpClient())
            {
                var url = CreateUrl(ORIGINALINTERACTIONURL, token);
                await client.DeleteAsync(url);
            }
        }

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

        private async Task<IEnumerable<Message>> ProcessRequest(IDiscordWebhookRequest webhookRequest, string webhookUrl, Func<IDiscordWebhookRequest, string, Task<Message?>> sendAction)
        {
            switch (webhookRequest)
            {
                case DiscordWebhookRequest discordWebhookRequest:
                    return await ProcessMessageToWebhook(discordWebhookRequest, webhookUrl, sendAction);
                case DiscordWebhookFilesRequest discordWebhookFiles:
                    return await ProcessFilesToWebhook(discordWebhookFiles, webhookUrl, sendAction);
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<IEnumerable<Message>> ProcessMessageToWebhook(DiscordWebhookRequest webhookRequest, string webhookUrl, Func<IDiscordWebhookRequest, string, Task<Message?>> sendAction)
        {
            var messageResults = new List<Message>();

            // Webhooks can (only) have 10 embeds
            if (webhookRequest.Embeds.Count() > 10)
            {
                foreach (var batch in webhookRequest.Embeds.Select((e, i) => new { e, i }).GroupBy(x => x.i / 10))
                {
                    var webhookBatch = new DiscordWebhookRequest
                    {
                        Content = webhookRequest.Content,
                        Username = webhookRequest.Username,
                        AvatarUrl = webhookRequest.AvatarUrl,
                        Embeds = batch.Select(x => x.e).ToList()
                    };

                    var message = await sendAction(webhookBatch, webhookUrl);
                    if (message != null)
                        messageResults.Add(message);
                }
            }
            else
            {
                var message = await sendAction(webhookRequest, webhookUrl);
                if (message != null)
                    messageResults.Add(message);
            }
            return messageResults;
        }

        private async Task<IEnumerable<Message>> ProcessFilesToWebhook(DiscordWebhookFilesRequest webhookRequest, string webhookUrl, Func<IDiscordWebhookRequest, string, Task<Message?>> sendAction)
        {
            var messageResults = new List<Message>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                // Webhooks can (only) have 10 embeds, but because embeds will most of the time have 2 files I will limit it to 5.
                // This also prevents a bug with this code that it can send more than 10 files if the count is less than 10 embeds
                if (webhookRequest.PayloadJson.Embeds.Count > 5)
                {
                    foreach (var batch in webhookRequest.PayloadJson.Embeds.Select((e, i) => new { e, i }).GroupBy(x => x.i / 10))
                    {
                        var webhookBatch = new DiscordWebhookFilesRequest
                        {
                            PayloadJson = new DiscordWebhookRequest
                            {
                                Content = webhookRequest.PayloadJson.Content,
                                Username = webhookRequest.PayloadJson.Username,
                                AvatarUrl = webhookRequest.PayloadJson.AvatarUrl,
                                Embeds = batch.Select(x => x.e).ToList()
                            }
                        };

                        var filesBelongingToEmbeds = webhookBatch.PayloadJson.Embeds.Where(x => x.Image?.Url.StartsWith("attachment://") == true).Select(x => x.Image.Url.Replace("attachment://", "")).ToArray()
                            .Concat(webhookBatch.PayloadJson.Embeds.Where(x => x.Auhor?.IconUrl.StartsWith("attachment://") == true).Select(x => x.Auhor.IconUrl.Replace("attachment://", "")).ToArray())
                            .Concat(webhookBatch.PayloadJson.Embeds.Where(x => x.Thumbnail?.Url.StartsWith("attachment://") == true).Select(x => x.Thumbnail.Url.Replace("attachment://", "")).ToArray())
                            .Distinct();

                        webhookBatch.Files = new Dictionary<string, byte[]>(webhookRequest.Files.Where(x => filesBelongingToEmbeds.Contains(x.Key)).ToArray());

                        // Max 10 files as well
                        if (webhookBatch.Files.Count > 10)
                        {
                            foreach (var fileBatch in webhookBatch.Files.Select((f, i) => new { f, i }).GroupBy(x => x.i / 10))
                            {
                                var webhookFileBatch = new DiscordWebhookFilesRequest
                                {
                                    PayloadJson = new DiscordWebhookRequest
                                    {
                                        Content = webhookBatch.PayloadJson.Content,
                                        Username = webhookBatch.PayloadJson.Username,
                                        AvatarUrl = webhookBatch.PayloadJson.AvatarUrl
                                    },
                                    Files = new Dictionary<string, byte[]>(fileBatch.Select(x => x.f).ToArray())
                                };

                                webhookFileBatch.PayloadJson.Embeds = webhookBatch.PayloadJson.Embeds.Where(x => webhookFileBatch.Files.Select(f => f.Key).All(f => filesBelongingToEmbeds.Contains(f))).ToList();

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
                    var message = await sendAction(webhookRequest, webhookUrl);
                    if (message != null)
                        messageResults.Add(message);
                }
            }

            return messageResults;
        }

        private async Task<Message?> SendPatchRequest(IDiscordWebhookRequest webhookRequest, string url)
        {
            _logger.LogDebug($"Sending patch request to '{url}'");

            switch (webhookRequest)
            {
                case DiscordWebhookRequest discordWebhookRequest:
                    return await SendPatchRequest(discordWebhookRequest, url);
                case DiscordWebhookFilesRequest discordWebhookFiles:
                    return await SendPatchRequest(discordWebhookFiles, url);
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<Message?> SendPatchRequest(DiscordWebhookRequest webhookRequest, string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PatchAsJsonAsync(url, webhookRequest);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format("Error posting to webhook, response code: {0}\\n\\tUrl: {1}\\n\\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(webhookRequest)), null, response.StatusCode);
                }

                return JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync());
            }
        }

        private async Task<Message?> SendPatchRequest(DiscordWebhookFilesRequest webhookRequest, string url)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("--boundary"))
                {
                    content.Add(new StringContent(JsonConvert.SerializeObject(webhookRequest.PayloadJson), Encoding.UTF8, "application/json"), "payload_json");
                    foreach (var file in webhookRequest.Files.Select((Data, Index) => (Index, Data)))
                        content.Add(CreateFileContent(file.Data.Value, "image/png"), $"files[{file.Index}]", file.Data.Key);

                    var response = await client.PatchAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(string.Format("Error posting to webhook, response code: {0}\\n\\tUrl: {1}\\n\\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(webhookRequest)), null, response.StatusCode);
                    }

                    return JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync());
                }
            }
        }

        private async Task<Message?> SendPostRequest(IDiscordWebhookRequest webhookRequest, string url)
        {
            _logger.LogDebug($"Sending post request to '{url}'");

            switch (webhookRequest)
            {
                case DiscordWebhookRequest discordWebhookRequest:
                    return await SendPostRequest(discordWebhookRequest, url);
                case DiscordWebhookFilesRequest discordWebhookFiles:
                    return await SendPostRequest(discordWebhookFiles, url);
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<Message?> SendPostRequest(DiscordWebhookRequest webhookRequest, string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(url, webhookRequest);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format("Error posting to webhook, response code: {0}\\n\\tUrl: {1}\\n\\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(webhookRequest)), null, response.StatusCode);
                }

                return JsonConvert.DeserializeObject<Message>(await response.Content.ReadAsStringAsync());
            }
        }

        private async Task<Message?> SendPostRequest(DiscordWebhookFilesRequest webhookRequest, string url)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("--boundary"))
                {
                    content.Add(new StringContent(JsonConvert.SerializeObject(webhookRequest.PayloadJson), Encoding.UTF8, "application/json"), "payload_json");
                    foreach (var file in webhookRequest.Files.Select((Data, Index) => (Index, Data)))
                        content.Add(CreateFileContent(file.Data.Value, "image/png"), $"files[{file.Index}]", file.Data.Key);

                    var response = await client.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(string.Format("Error posting to webhook, response code: {0}\\n\\tUrl: {1}\\n\\tData: {2}", response.StatusCode, url, JsonConvert.SerializeObject(webhookRequest)), null, response.StatusCode);
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
