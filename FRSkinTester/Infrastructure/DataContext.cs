using FRTools.Infrastructure.DataModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FRTools.Infrastructure
{
    // Is EF for this project overkilll? Yes, yes it is.
    // But I cba maintaining db scripts if I ever expand this project later.
    public class DataContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public DataContext() : base("defaultConnection")
        {
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        public DbSet<Skin> Skins { get; set; }
        public DbSet<DragonCache> DragonCache { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
        }
    }
}