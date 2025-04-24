using Microsoft.EntityFrameworkCore;
using SurfSwift.WorkerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfSwift.WorkerService.Context
{
    public class SurfSwiftDbContext : DbContext
    {
        public SurfSwiftDbContext(DbContextOptions<SurfSwiftDbContext> options)
                    : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<ActionProject> ActionProjects { get; set; }
        public DbSet<ActionTemplate> ActionTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // User Table Mapping
            modelBuilder.Entity<User>().ToTable("tblUsers");

            // Project Table Mapping
            modelBuilder.Entity<ActionProject>().ToTable("tblProjects");

            // ActionTemplate Table Mapping
            modelBuilder.Entity<ActionTemplate>().ToTable("tblActionTemplates");

            modelBuilder.Entity<ActionProject>()
           .HasOne(p => p.CreatedByUser)
           .WithMany()
           .HasForeignKey(p => p.CreatedByUserId)
           .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
