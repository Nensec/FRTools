using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Data.DataModels.DiscordModels
{
    public class DiscordSetting
    {
        public int Id { get; set; }
        public DiscordServer Server { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
