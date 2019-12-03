using System;

namespace FRTools.Data.DataModels.FlightRisingModels
{
    public class FRUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int FRId { get; set; }
        public User User { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
