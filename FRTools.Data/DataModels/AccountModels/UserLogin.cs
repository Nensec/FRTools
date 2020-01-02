using Microsoft.AspNet.Identity.EntityFramework;

namespace FRTools.Data.DataModels
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public string ProviderUsername { get; set; }
    }
}