using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.MessageModels
{
    public class Reaction
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("count_details")]
        public CountDetails CountDetails { get; set; }
        [JsonProperty("me")]
        public bool Me { get; set; }
        [JsonProperty("me_burst")]
        public bool MeSuper { get; set; }
        [JsonProperty("emoji")]
        public Emoji Emoji { get; set; }
        [JsonProperty("burst_colors")]
        public IEnumerable<int> SuperColors { get; set; } = Enumerable.Empty<int>();
    }

    public class CountDetails
    {
        [JsonProperty("burst")]
        public int Super { get; set; }
        [JsonProperty("normal")]
        public int Normal { get; set; }
    }
}
