using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.UserModels
{
    public class User
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }
        [JsonProperty("public_flags")]
        public int PublicFlags { get; set; }
    }
}
