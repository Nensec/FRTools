using Microsoft.AspNet.Identity.EntityFramework;

namespace FRSkinTester.Infrastructure.DataModels
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public int? FRId { get; set; }
        public string FRName { get; set; }
    }
}