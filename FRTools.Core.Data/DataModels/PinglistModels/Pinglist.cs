using FRTools.Core.Data.DataModels.AccountModels;

namespace FRTools.Core.Data.DataModels.PinglistModels
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
        public virtual PinglistCategory PinglistCategory { get; set; }
    }
}
