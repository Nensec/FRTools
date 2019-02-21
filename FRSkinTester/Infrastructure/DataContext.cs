using FRSkinTester.Infrastructure.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FRSkinTester.Infrastructure
{
    // Is EF for this project overkilll? Yes, yes it is.
    // But I cba maintaining db scripts if I ever expand this project later.
    public class DataContext : DbContext
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
    }
}