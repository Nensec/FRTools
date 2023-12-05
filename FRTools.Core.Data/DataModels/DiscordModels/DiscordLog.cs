using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Core.Data.DataModels.DiscordModels
{
    public class DiscordLog
    {
        public int Id { get; set; }
        [Required]
        public long UserId { get; set; }
        public long ChannelId { get; set; }
        public string Module { get; set; }
        public string Command { get; set; }
        public string Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
