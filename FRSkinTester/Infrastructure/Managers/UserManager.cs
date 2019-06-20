using FRTools.Infrastructure.DataModels;
using Microsoft.AspNet.Identity;

namespace FRTools.Infrastructure.Managers
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store) : base(store)
        {
        }
    }
}