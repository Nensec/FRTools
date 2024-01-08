using Newtonsoft.Json;

namespace FRTools.Core.Services.Discord.DiscordModels.MessageModels
{
    public class MessageReference
    {
        [JsonProperty("message_id")]
        public ulong MessageId { get; set; }
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }
        [JsonProperty("fail_if_not_exists")]
        public bool FailIfNotExists { get; set; }
    }
}
