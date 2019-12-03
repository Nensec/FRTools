using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Data.DataModels.DiscordModels
{
    public class DiscordChannel
    {
        public int Id { get; set; }
        public DiscordServer Server { get; set; }
        public long ChannelId { get; set; }
        public string Name { get; set; }
    }
}
