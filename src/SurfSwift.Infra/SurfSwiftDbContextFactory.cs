using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SurfSwift.Infra
{
    public class SurfSwiftDbContextFactory : IDesignTimeDbContextFactory<SurfSwiftDbContext>
    {
        public SurfSwiftDbContext CreateDbContext(string[] args)
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, "SurfSwift.db");

            var optionsBuilder = new DbContextOptionsBuilder<SurfSwiftDbContext>();
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new SurfSwiftDbContext(optionsBuilder.Options);
        }
    }
}
