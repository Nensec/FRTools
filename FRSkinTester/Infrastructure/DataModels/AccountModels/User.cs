using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace FRSkinTester.Infrastructure.DataModels
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public int? FRId { get; set; }
        public string FRName { get; set; }

        public virtual ICollection<Skin> Skins { get; set; } = new HashSet<Skin>();
    }
}