using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Data.DataModels.DiscordModels
{
    public class DiscordServerUser
    {
        public int Id { get; set; }
        public DiscordServer Server { get; set; }
        public DiscordUser User { get; set; }
        public string Nickname { get; set; }
        public ICollection<DiscordRole> Roles { get; set; } = new HashSet<DiscordRole>();
    }
}
