namespace FRTools.Core.Services.DiscordModels
{
    public class AppCommand
    {
        public ulong id { get; set; }
        public ulong application_id { get; set; }
        public ulong? guild_id { get; set; }
        public ulong version { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public AppCommandType type { get; set; } = AppCommandType.CHAT_INPUT;
        public AppCommandOption[] options { get; set; }
        public Permissions default_member_permissions { get; set; }
        public bool? dm_permission { get; set; }
    }

    public class AppCommandOption
    {
        public string name { get; set; }
        public string description { get; set; }
        public AppCommandOptionType type { get; set; }
        public bool? required { get; set; }
        public AppCommandOptionChoice[] choices { get; set; }
        public AppCommandOption[] options { get; set; }
        public ChannelType[] channel_types { get; set; }
        public int? min_value { get; set; }
        public int? max_value { get; set; }
        public int? min_length { get; set; }
        public int? max_length { get; set; }
        private bool? _autocomplete;
        public bool? autocomplete
        {
            get => choices.Any() ? false : _autocomplete;
            set => _autocomplete = value;
        }
    }

    public abstract class AppCommandOptionChoice
    {
        public string name { get; set; }
        public object value { get; set; }
    }

    public class AppCommandOptionChoice<T> : AppCommandOptionChoice
    {
        public new T value { get; set; }
    }
}
