using Microsoft.EntityFrameworkCore;
using SurfSwift.Entities;

namespace SurfSwift.Infra
{
    public class SurfSwiftDbContext : DbContext
    {
        public SurfSwiftDbContext(DbContextOptions<SurfSwiftDbContext> options)
          : base(options)
        {
        }

        public DbSet<ConfigurationItem> Configurations { get; set; }

        public DbSet<ActionScript> ActionScripts { get; set; }

        public DbSet<UserData> ReplacementData { get; set; }
    }
}
