using FRTools.Core.Services.Discord.DiscordModels.GuildModels;
using FRTools.Core.Services.Discord.DiscordModels.UserModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.WebhookModels
{
    public class DiscordWebhookResponse
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("application_id")]
        public ulong ApplicationId { get; set; }
        [JsonProperty("source_guild")]
        public Guild Guild { get; set; }
        [JsonProperty("source_channel")]
        public Channel Channel { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
