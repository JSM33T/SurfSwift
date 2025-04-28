using Microsoft.EntityFrameworkCore;
using SurfSwift.Entities;

namespace SurfSwift.Infra
{
    public class SurfSwiftDbContext : DbContext
    {
        public SurfSwiftDbContext(DbContextOptions<SurfSwiftDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ActionProject> ActionProjects { get; set; }
        public DbSet<ActionTemplate> ActionTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Fluent API if needed
        }
    }
}
