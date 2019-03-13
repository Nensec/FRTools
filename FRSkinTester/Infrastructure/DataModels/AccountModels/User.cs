using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace FRSkinTester.Infrastructure.DataModels
{
    [Flags]
    public enum Privacy
    {
        HidePreviews = 1,
        HideSkins = 2,
        HideAll = 3
    }

    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public int? FRId { get; set; }
        public string FRName { get; set; }

        public Privacy Privacy { get; set; }

        public virtual ICollection<Skin> Skins { get; set; } = new HashSet<Skin>();
    }
}