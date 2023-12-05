using System.ComponentModel.DataAnnotations;
using FRTools.Core.Data.DataModels.FlightRisingModels;

namespace FRTools.Core.Data.DataModels.PinglistModels
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