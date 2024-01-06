using FRTools.Core.Services.Discord.DiscordModels.GuildModels;
using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels
{
    public class DiscordInteractionRequest
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("application_id")]
        public ulong ApplicationId { get; set; }
        [JsonProperty("type")]
        public InteractionType Type { get; set; }
        [JsonProperty("data")]
        public DiscordInteractionRequestData Data { get; set; }
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }
        [JsonProperty("member")]
        public Member Member { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("app_permissions")]
        public Permissions AppPermissions { get; set; }
    }

    public class DiscordInteractionRequestData
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("options")]
        public IEnumerable<DiscordInteractionRequestOptionData> Options { get; set; } = Enumerable.Empty<DiscordInteractionRequestOptionData>();
        [JsonProperty("type")]
        public AppCommandType Type { get; set; }
    }

    public class DiscordInteractionRequestOptionData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public object Value { get; set; }
        [JsonProperty("options")]
        public IEnumerable<DiscordInteractionRequestOptionData> Options { get; set; } = Enumerable.Empty<DiscordInteractionRequestOptionData>();
        [JsonProperty("type")]
        public AppCommandOptionType Type { get; set; }
    }
}
