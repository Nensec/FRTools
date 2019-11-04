using FRTools.Data.DataModels;
using FRTools.Data.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace FRTools.Data
{
    // Is EF for this project overkilll? Yes, yes it is.
    // But I cba maintaining db scripts if I ever expand this project later.
    public class DataContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        static DataContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
        }

        public DataContext() : base("defaultConnection")
        {
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        public DbSet<Skin> Skins { get; set; }
        public DbSet<DragonCache> DragonCache { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Pinglist> Pinglists { get; set; }
        public DbSet<FRUser> FRUsers { get; set; }
        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");

            modelBuilder.Entity<User>().HasOptional(x => x.FRUser).WithOptionalDependent(x => x.User);
        }
    }
}