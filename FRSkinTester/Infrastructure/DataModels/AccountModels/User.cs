using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace FRSkinTester.Infrastructure.DataModels
{

    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public int? FRId { get; set; }
        public string FRName { get; set; }

        public Privacy Privacy { get; set; }
        public SkinVisiblity DefaultVisibility { get; set; }

        public virtual ICollection<Skin> Skins { get; set; } = new HashSet<Skin>();
        public virtual ICollection<Preview> Previews { get; set; } = new HashSet<Preview>();
    }
}