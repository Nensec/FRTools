using FRTools.Core.Services.Discord.DiscordModels.Embed;
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
    }

    public class DiscordWebhookFiles
    {
        [JsonProperty("payload_json")]
        public DiscordWebhook PayloadJson { get; set; }

        [JsonProperty("files")]
        public IEnumerable<(byte[] file, string name)> Files { get; set; }
    }
}
