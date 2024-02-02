using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels
{
    public class DiscordInteractionSelectOption
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("description")]
        public string? Description { get; set; }
        [JsonProperty("emoji")]
        public Emoji? Emoji { get; set; }
        [JsonProperty("default")]
        public bool? Default { get; set; } = false;
    }
}
