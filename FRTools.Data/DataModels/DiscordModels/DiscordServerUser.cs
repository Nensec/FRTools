using System.Collections.Generic;

namespace FRTools.Data.DataModels.DiscordModels
{
    public class DiscordServerUser
    {
        public int Id { get; set; }
        public virtual DiscordServer Server { get; set; }
        public virtual DiscordUser User { get; set; }
        public string Nickname { get; set; }
        public bool IsOwner { get; set; }
        public virtual ICollection<DiscordRole> Roles { get; set; } = new HashSet<DiscordRole>();
    }
}
