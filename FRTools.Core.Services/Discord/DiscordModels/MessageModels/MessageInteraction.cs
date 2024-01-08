using FRTools.Core.Services.Discord.DiscordModels.GuildModels;
using FRTools.Core.Services.Discord.DiscordModels.UserModels;
using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.MessageModels
{
    public class MessageInteraction
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("type")]
        public InteractionType Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("member")]
        public Member Member { get; set; }
    }
}
