using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.MessageModels
{
    public class Activity
    {
        [JsonProperty("type")]
        public MessageActivity Type { get; set; }
        [JsonProperty("party_id")]
        public string PartyId { get; set; }
    }
}
