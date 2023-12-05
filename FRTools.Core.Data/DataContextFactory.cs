using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FRTools.Core.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_defaultConnection"));

            return new DataContext(optionsBuilder.Options);
        }
    }
}
