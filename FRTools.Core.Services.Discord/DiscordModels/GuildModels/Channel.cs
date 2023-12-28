using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.GuildModels
{
    public class Channel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("type")]
        public ChannelType Type { get; set; }
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

}
