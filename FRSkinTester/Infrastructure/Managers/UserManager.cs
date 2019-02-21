using FRSkinTester.Infrastructure.DataModels;
using Microsoft.AspNet.Identity;

namespace FRSkinTester.Infrastructure.Managers
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store) : base(store)
        {
        }
    }
}