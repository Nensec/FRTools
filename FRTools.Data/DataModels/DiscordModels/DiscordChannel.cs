using System.Collections.Generic;

namespace FRTools.Data.DataModels.DiscordModels
{
    public enum DiscordChannelType
    {
        Text, Voice, Category, Other
    }

    public class DiscordChannel
    {
        public int Id { get; set; }
        public DiscordServer Server { get; set; }
        public long ChannelId { get; set; }
        public string Name { get; set; }
        public DiscordChannelType ChannelType { get; set; }
        public virtual ICollection<DiscordLog> Logs { get; set; } = new HashSet<DiscordLog>();
    }
}
