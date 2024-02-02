using FRTools.Core.Services.Discord.DiscordModels.GuildModels;
using FRTools.Core.Services.Discord.DiscordModels.MessageModels;
using FRTools.Core.Services.Discord.DiscordModels.UserModels;
using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.InteractionRequestModels
{
    public class DiscordInteractionRequest
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("application_id")]
        public ulong ApplicationId { get; set; }
        [JsonProperty("type")]
        public InteractionType Type { get; set; }
        [JsonProperty("data")]
        public DiscordInteractionRequestData Data { get; set; }
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }
        [JsonProperty("member")]
        public Member Member { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("app_permissions")]
        public Permissions AppPermissions { get; set; }
    }

    public class DiscordInteractionRequestData
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("options")]
        public IEnumerable<DiscordInteractionRequestOptionData> Options { get; set; } = Enumerable.Empty<DiscordInteractionRequestOptionData>();
        [JsonProperty("type")]
        public AppCommandType Type { get; set; }
        [JsonProperty("custom_id")]
        public string CustomId { get; set; }
        [JsonProperty("component_type")]
        public ComponentType ComponentType { get; set; }
        [JsonProperty("values")]
        public IEnumerable<string> Values { get; set; }
        [JsonProperty("resolved")]
        public DiscordInteractionRequestResolvedData ResolvedData { get; set; }
    }

    public class DiscordInteractionRequestOptionData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public object Value { get; set; }
        [JsonProperty("options")]
        public IEnumerable<DiscordInteractionRequestOptionData> Options { get; set; } = Enumerable.Empty<DiscordInteractionRequestOptionData>();
        [JsonProperty("type")]
        public AppCommandOptionType Type { get; set; }
    }

    public class DiscordInteractionRequestResolvedData
    {
        [JsonProperty("users")]
        [JsonConverter(typeof(SingleOrArrayConverter<User>))]
        public IEnumerable<User> Users { get; set; }
        [JsonProperty("members")]
        [JsonConverter(typeof(SingleOrArrayConverter<Member>))]
        public IEnumerable<Member> Members { get; set; }
        [JsonProperty("roles")]
        [JsonConverter(typeof(SingleOrArrayConverter<Role>))]
        public IEnumerable<Role> Roles { get; set; }
        [JsonProperty("channels")]
        [JsonConverter(typeof(SingleOrArrayConverter<Channel>))]
        public IEnumerable<Channel> Channels { get; set; }
        [JsonProperty("messages")]
        [JsonConverter(typeof(SingleOrArrayConverter<Message>))]
        public IEnumerable<Message> Messages { get; set; }
        [JsonProperty("attachments")]
        [JsonConverter(typeof(SingleOrArrayConverter<Attachment>))]
        public IEnumerable<Attachment> Attachments { get; set; }
    }
}
