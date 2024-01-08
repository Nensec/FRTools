using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.MessageModels
{
    public class Attachment
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("filename")]
        public string Filename { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("proxy_url")]
        public string ProxyUrl { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("ephemeral")]
        public bool Ephemeral { get; set; }
        [JsonProperty("duration_secs")]
        public double AudioDuration { get; set; }
        [JsonProperty("waveform")]
        public string Waveform { get; set; }
        [JsonProperty("flags")]
        public int Flags { get; set; }
    }
}
