using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.GuildModels
{
    public class Role
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("color")]
        public int Color { get; set; }
        [JsonProperty("hoist")]
        public bool Hoist { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("unicode_emoji")]
        public string UnicodeEmoji { get; set; }
        [JsonProperty("position")]
        public int Position { get; set; }
        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }
        [JsonProperty("managed")]
        public bool Managed { get; set; }
        [JsonProperty("mentionable")]
        public bool Mentionable { get; set; }
        [JsonProperty("tags")]
        public IEnumerable<RoleTag> Tags { get; set; } = Enumerable.Empty<RoleTag>();
        [JsonProperty("flags")]
        public int Flags { get; set; }
    }
}
