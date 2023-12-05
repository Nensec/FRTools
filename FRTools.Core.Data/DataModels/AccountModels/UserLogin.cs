using Microsoft.AspNetCore.Identity;

namespace FRTools.Core.Data.DataModels.AccountModels
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public string ProviderUsername { get; set; }
    }
}