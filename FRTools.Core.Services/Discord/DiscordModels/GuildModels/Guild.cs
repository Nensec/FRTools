using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.GuildModels
{
    public class Guild
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
