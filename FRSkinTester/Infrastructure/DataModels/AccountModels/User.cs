using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FRTools.Infrastructure.DataModels
{
    [Flags]
    public enum Privacy
    {
        [Description("Show all")]
        ShowAll = 0,
        [Description("Hide previews")]
        HidePreviews = 1,
        [Description("Hide skins")]
        HideSkins = 2,
        [Description("Make profile private")]
        HideAll = 3
    }

    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public int? FRId { get; set; }
        public string FRName { get; set; }

        public Privacy Privacy { get; set; }

        public virtual ICollection<Skin> Skins { get; set; } = new HashSet<Skin>();
        public virtual ICollection<Preview> Previews { get; set; } = new HashSet<Preview>();
    }
}