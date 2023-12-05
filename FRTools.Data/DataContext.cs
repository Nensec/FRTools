using System.Data.Entity;
using FRTools.Data.DataModels;
using FRTools.Data.DataModels.DiscordModels;
using FRTools.Data.DataModels.FlightRisingModels;
using FRTools.Data.DataModels.NewsReaderModels;
using FRTools.Data.DataModels.PinglistModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FRTools.Data
{
    // Is EF for this project overkilll? Yes, yes it is.
    // But I cba maintaining db scripts if I ever expand this project later.
    public class DataContext : IdentityDbContext<User, IdentityRole<int, IdentityUserRole<int>>, int, UserLogin, IdentityUserRole<int>, IdentityUserClaim<int>>
    {
        public DataContext() : base("defaultConnection")
        {
        }

        // FRTools tables
        public IDbSet<Job> Jobs { get; set; }
        public IDbSet<ProfileSettings> ProfileSettings { get; set; }

        // Skintester tables
        public IDbSet<Skin> Skins { get; set; }
        public IDbSet<Preview> Previews { get; set; }

        // Newsreader tables
        public IDbSet<Topic> Topics { get; set; }
        public IDbSet<Post> Posts { get; set; }

        // Pinglist tables
        public IDbSet<Pinglist> Pinglists { get; set; }
        public IDbSet<PingListEntry> PingListEntries { get; set; }
        public IDbSet<PinglistCategory> PinglistCategories { get; set; }

        // Flight rising tables
        public IDbSet<DragonCache> DragonCache { get; set; }
        public IDbSet<FRUser> FRUsers { get; set; }
        public IDbSet<FRDominance> FRDominances { get; set; }
        public IDbSet<FRItem> FRItems { get; set; }
        public IDbSet<FRItemFlashSale> FRItemFlashSales { get; set; }

        // Discord tables
        public IDbSet<DiscordLog> DiscordLogs { get; set; }
        public IDbSet<DiscordServer> DiscordServers { get; set; }
        public IDbSet<DiscordSetting> DiscordSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");

            modelBuilder.Entity<User>().HasOptional(x => x.FRUser).WithOptionalDependent(x => x.User);
            modelBuilder.Entity<User>().HasOptional(x => x.ProfileSettings).WithOptionalPrincipal(x => x.User);
        }
    }
}