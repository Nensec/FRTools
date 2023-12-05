using System.Collections.Generic;
using FRTools.Core.Data.DataModels.AccountModels;

namespace FRTools.Core.Data.DataModels.PinglistModels
{
    public class PinglistCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Pinglist> Pinglists { get; set; }
    }
}
