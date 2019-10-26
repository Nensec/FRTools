using System.ComponentModel.DataAnnotations;

namespace FRTools.Data.DataModels
{
    public class PingListEntry
    {
        public int Id { get; set; }
        [Required]
        public virtual FRUser FRUser { get; set; }

        public string GeneratedId { get; set; }
        public string SecretKey { get; set; }
        public string Remarks { get; set; }
    }
}