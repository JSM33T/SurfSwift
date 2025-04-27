using Microsoft.EntityFrameworkCore;
using SurfSwift.DBEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfSwift.DBEngine.Context
{
    public class SurfSwiftDbContext : DbContext
    {
        public SurfSwiftDbContext(DbContextOptions<SurfSwiftDbContext> options)
                    : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<ActionProject> ActionProject { get; set; }
        public DbSet<ActionTemplate> ActionTemplate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // User Table Mapping
            modelBuilder.Entity<User>().ToTable("tblUser");

            // Project Table Mapping
            modelBuilder.Entity<ActionProject>().ToTable("tblActionProject");

            // ActionTemplate Table Mapping
            modelBuilder.Entity<ActionTemplate>().ToTable("tblActionTemplate");

        }
    }
}
