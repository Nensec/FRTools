using FRTools.Core.Services.Discord.DiscordModels.Embed;
using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.WebhookModels
{
    public class DiscordWebhook
    {
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonProperty("embeds")]
        public IEnumerable<DiscordEmbed> Embeds { get; set; } = Enumerable.Empty<DiscordEmbed>();
        [JsonProperty("flags")]
        public MessageFlags Flags { get; set; }
    }

    public class DiscordWebhookFiles
    {
        [JsonProperty("payload_json")]
        public DiscordWebhook PayloadJson { get; set; } = new DiscordWebhook();

        [JsonProperty("files")]
        public Dictionary<string, byte[]> Files { get; set; } = new Dictionary<string, byte[]>();
    }
}
