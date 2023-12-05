using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Data.DataModels.PinglistModels;
using Microsoft.AspNetCore.Identity;

namespace FRTools.Core.Data.DataModels.AccountModels
{

    public class User : IdentityUser<int>
    {
        public virtual ProfileSettings ProfileSettings { get; set; }
        public virtual FRUser FRUser { get; set; }
        public virtual ICollection<Skin> Skins { get; set; } = new HashSet<Skin>();
        public virtual ICollection<Preview> Previews { get; set; } = new HashSet<Preview>();
        public virtual ICollection<Pinglist> Pinglists { get; set; } = new HashSet<Pinglist>();
    }
}