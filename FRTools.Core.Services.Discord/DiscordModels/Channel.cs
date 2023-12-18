namespace FRTools.Core.Services.DiscordModels
{
    public class Channel
    {
        public ulong id { get; set; }
        public ChannelType type { get; set; }
        public ulong guild_id { get; set; }
        public string name { get; set; }
    }

}
