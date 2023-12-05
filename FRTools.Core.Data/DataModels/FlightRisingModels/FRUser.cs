using System;
using FRTools.Core.Data.DataModels.AccountModels;

namespace FRTools.Core.Data.DataModels.FlightRisingModels
{
    public class FRUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int FRId { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
