using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.Embed
{
    public class DiscordEmbed
    {
        [JsonProperty("auhor")]
        public DiscordEmbedAuthor Auhor { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("color")]
        public int Color { get; set; }
        [JsonProperty("fields")]
        public IEnumerable<DiscordEmbedField> Fields { get; set; } = Enumerable.Empty<DiscordEmbedField>();
        [JsonProperty("thumbnail")]
        public DiscordEmbedThumbnail Thumbnail { get; set; }
        [JsonProperty("image")]
        public DiscordEmbedImage Image { get; set; }
        [JsonProperty("footer")]
        public DiscordEmbedFooter Footer { get; set; }
        [JsonProperty("timestamp")]
        public DateTime? Timestamp { get; set; }
    }

    public class DiscordEmbedAuthor
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
    }

    public class DiscordEmbedField
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("inline")]
        public bool Inline { get; set; }
    }

    public class DiscordEmbedThumbnail
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class DiscordEmbedImage
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class DiscordEmbedFooter
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
    }
}
