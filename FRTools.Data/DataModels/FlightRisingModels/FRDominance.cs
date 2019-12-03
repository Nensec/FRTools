using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Data.DataModels.FlightRisingModels
{
    public class FRDominance
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int First { get; set; }
        public int Second { get; set; }
        public int Third { get; set; }
    }
}
