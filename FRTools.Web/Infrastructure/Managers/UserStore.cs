using FRTools.Data.DataModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer.Utilities;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FRTools.Web.Infrastructure.Managers
{
    public class UserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        private bool _disposed;

        public UserStore(DbContext context) : base(context)
        {
        }

        public override Task AddLoginAsync(User user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            var logins = Context.Set<UserLogin>();
            var providerData = JsonConvert.DeserializeObject<ProviderData>(login.ProviderKey);
            logins.Add(new UserLogin
            {
                UserId = user.Id,
                ProviderKey = providerData.ProviderKey,
                ProviderUsername = providerData.ProviderUsername,
                LoginProvider = login.LoginProvider,
            });
            return Task.FromResult(0);
        }

        public override async  Task<User> FindAsync(UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            var providerData = JsonConvert.DeserializeObject<ProviderData>(login.ProviderKey);
            var logins = Context.Set<UserLogin>();
            var provider = login.LoginProvider;
            var key = providerData.ProviderKey;
            var userLogin =
                await logins.FirstOrDefaultAsync(l => l.LoginProvider == provider && l.ProviderKey == key).WithCurrentCulture();
            if (userLogin != null)
            {
                var userId = userLogin.UserId;
                return await GetUserAggregateAsync(u => u.Id.Equals(userId)).WithCurrentCulture();
            }
            return null;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = true;
            base.Dispose(disposing);
        }

        public class ProviderData
        {
            public string ProviderKey { get; set; }
            public string ProviderUsername { get; set; }
            public string AccessToken { get; set; }
        }
    }
}