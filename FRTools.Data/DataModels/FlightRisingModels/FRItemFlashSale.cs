using System;

namespace FRTools.Data.DataModels.FlightRisingModels
{
    public class FRItemFlashSale
    {
        public int Id { get; set; }
        public virtual FRItem Item { get; set; }
        public DateTime DiscoveredAt { get; set; }
        public DateTime? RemovedAt { get; set; }
    }
}
