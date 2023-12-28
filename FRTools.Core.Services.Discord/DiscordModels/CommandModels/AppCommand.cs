using FRTools.Core.Services.DiscordModels;
using Newtonsoft.Json;

#pragma warning disable CS8618
namespace FRTools.Core.Services.Discord.DiscordModels.CommandModels
{
    public class AppCommand
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("type")]
        public AppCommandType Type { get; set; } = AppCommandType.CHAT_INPUT;
        [JsonProperty("options")]
        public IEnumerable<AppCommandOption> Options { get; set; } = Enumerable.Empty<AppCommandOption>();
        [JsonProperty("default_member_permissions")]
        public Permissions DefaultMemberPermissions { get; set; }
        [JsonProperty("dm_permission")]
        public bool? DmPermission { get; set; } = false;
    }

    public class AppCommandOption
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("type")]
        public AppCommandOptionType Type { get; set; } = AppCommandOptionType.STRING;
        [JsonProperty("required")]
        public bool? Required { get; set; }
        [JsonProperty("choices")]
        public IEnumerable<AppCommandOptionChoice> Choices { get; set; } = Enumerable.Empty<AppCommandOptionChoice>();
        [JsonProperty("options")]
        public IEnumerable<AppCommandOption> Options { get; set; } = Enumerable.Empty<AppCommandOption>();
        [JsonProperty("channel_types")]
        public IEnumerable<ChannelType> ChannelTypes { get; set; } = Enumerable.Empty<ChannelType>();
        [JsonProperty("min_value")]
        public int? Min_value { get; set; }
        [JsonProperty("max_value")]
        public int? Max_value { get; set; }
        [JsonProperty("min_length")]
        public int? Min_length { get; set; }
        [JsonProperty("max_length")]
        public int? Max_length { get; set; }
        private bool? _autocomplete;
        [JsonProperty("autocomplete")]
        public bool? Autocomplete
        {
            get => Choices.Any() ? false : _autocomplete;
            set => _autocomplete = value;
        }
    }

    public class AppCommandOptionChoice
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public object Value { get; set; }
    }
}
