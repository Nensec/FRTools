using FRTools.Data.DataModels.FlightRisingModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace FRTools.Data.DataModels
{

    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public Privacy Privacy { get; set; }
        public SkinVisiblity DefaultVisibility { get; set; }

        public virtual FRUser FRUser { get; set; }
        public virtual ICollection<Skin> Skins { get; set; } = new HashSet<Skin>();
        public virtual ICollection<Preview> Previews { get; set; } = new HashSet<Preview>();
    }
}