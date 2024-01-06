using FRTools.Core.Services.Discord.DiscordModels.UserModels;
using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.GuildModels
{
    public class Member
    {
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("roles")]
        public ulong[] Roles { get; set; }
        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }
        [JsonProperty("nick")]
        public object Nick { get; set; }
        [JsonProperty("joined_at")]
        public DateTime JoinedAt { get; set; }
    }
}
