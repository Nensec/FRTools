using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.WebhookModels
{
    public class DiscordWebhookRequest : IDiscordWebhookRequest
    {
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; } = Environment.GetEnvironmentVariable("DefaultDiscordUsername")!;
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; } = Environment.GetEnvironmentVariable("DefaultDiscordAvatarUrl")!;
        [JsonProperty("embeds")]
        public List<DiscordEmbed> Embeds { get; set; } = new List<DiscordEmbed>();
        [JsonProperty("flags")]
        public MessageFlags Flags { get; set; }
    }

    public class DiscordWebhookFilesRequest : IDiscordWebhookRequest
    {
        [JsonProperty("payload_json")]
        public DiscordWebhookRequest PayloadJson { get; set; } = new DiscordWebhookRequest();

        [JsonProperty("files")]
        public Dictionary<string, byte[]> Files { get; set; } = new Dictionary<string, byte[]>();
    }

    public interface IDiscordWebhookRequest
    {
    }
}
