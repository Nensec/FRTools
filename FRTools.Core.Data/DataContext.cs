﻿using FRTools.Core.Data.DataModels;
using FRTools.Core.Data.DataModels.AccountModels;
using FRTools.Core.Data.DataModels.DiscordModels;
using FRTools.Core.Data.DataModels.FlightRisingModels;
using FRTools.Core.Data.DataModels.NewsReaderModels;
using FRTools.Core.Data.DataModels.PinglistModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FRTools.Core.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {            
        }

        // FRTools tables
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<ProfileSettings> ProfileSettings { get; set; }

        // Skintester tables
        public virtual DbSet<Skin> Skins { get; set; }
        public virtual DbSet<Preview> Previews { get; set; }

        // Newsreader tables
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        // Pinglist tables
        public virtual DbSet<Pinglist> Pinglists { get; set; }
        public virtual DbSet<PingListEntry> PingListEntries { get; set; }
        public virtual DbSet<PinglistCategory> PinglistCategories { get; set; }

        // Flight rising tables
        public virtual DbSet<DragonCache> DragonCache { get; set; }
        public virtual DbSet<FRUser> FRUsers { get; set; }
        public virtual DbSet<FRDominance> FRDominances { get; set; }
        public virtual DbSet<FRItem> FRItems { get; set; }
        public virtual DbSet<FRItemFlashSale> FRItemFlashSales { get; set; }

        // Discord tables
        public virtual DbSet<DiscordLog> DiscordLogs { get; set; }
        public virtual DbSet<DiscordServer> DiscordServers { get; set; }
        public virtual DbSet<DiscordSetting> DiscordSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(property => property.IsForeignKey() && !property.DeclaringEntityType.IsOwned());

                foreach (var property in properties)
                {
                    var newName = GetNewColumnName(property.Name);

                    property.SetColumnName(newName);
                }
            }
        }

        private static string GetNewColumnName(string name)
        {
            if (name.Length > 2 && name[^2..] == "Id" && name[^3..] != "_Id")
            {
                return $"{name[0..^2]}_Id";
            }

            return name;
        }
    }
}
