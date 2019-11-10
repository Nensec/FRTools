using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRTools.Data.DataModels
{
    public class PinglistCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Pinglist> Pinglists { get; set; }
        public User Owner { get; set; }
    }
}
