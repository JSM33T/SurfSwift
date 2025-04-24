using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using SurfSwift.WorkerService.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfSwift.WorkerService
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SurfSwiftDbContext>
    {
        public SurfSwiftDbContext CreateDbContext(string[] args)
        {
            // Build configuration to access appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SurfSwiftDbContext>();

            // Get the connection string from the appsettings.json
            var connectionString = configuration.GetSection("AppSetting")["ConnectionString"];


            // Configure the DbContext to use SQL Server
            optionsBuilder.UseSqlServer(connectionString);

            return new SurfSwiftDbContext(optionsBuilder.Options);
        }
    }
}
