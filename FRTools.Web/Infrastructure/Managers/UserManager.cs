using System;
using FRTools.Data.DataModels;
using Microsoft.AspNet.Identity;

namespace FRTools.Web.Infrastructure.Managers
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store) : base(store)
        {
        }
    }
}