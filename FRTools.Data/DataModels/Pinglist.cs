using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Data.DataModels
{
    public class Pinglist
    {
        public int Id { get; set; }
        public string GeneratedId { get; set; }
        public string SecretKey { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public string Format { get; set; }
        public virtual User Creator { get; set; }

        public virtual ICollection<PingListEntry> Entries { get; set; } = new HashSet<PingListEntry>();
    }
}
