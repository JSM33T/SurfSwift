using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using SurfSwift.DBEngine.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurfSwift.DBEngine.ListDTO;
using Microsoft.Extensions.Configuration;

namespace SurfSwift.DBEngine
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SurfSwiftDbContext>
    {
        public SurfSwiftDbContext CreateDbContext(string[] args)
        {
            // Get the path to the assembly where this code is running
            var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

            // Navigate up to the project root (adjust as needed for your structure)
            var projectRoot = Path.GetFullPath(Path.Combine(assemblyDirectory, "../../.."));

            Console.WriteLine($"Looking for appsettings.json in: {projectRoot}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectRoot)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var configDto = configuration.GetSection("AppSetting").Get<ConfigurationListDTO>();

            var optionsBuilder = new DbContextOptionsBuilder<SurfSwiftDbContext>();
            optionsBuilder.UseSqlServer(configDto.ConnectionString);

            return new SurfSwiftDbContext(optionsBuilder.Options);
        }
    }
}