using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;



namespace SolaraNet.DataAccessLayer.EntityFramework.DBContext
{
    public class DBContextFactory : IDesignTimeDbContextFactory<SolaraNetDBContext>
    {
        public SolaraNetDBContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SolaraNetDBContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("SolaraNetDb"), builder =>
                    builder.MigrationsAssembly(
            typeof(DataAccessModel).Assembly.FullName)
            //typeof(SolaraNetDBContextModelSnapshot).Assembly.FullName)
            );
            return new SolaraNetDBContext(optionsBuilder.Options);
        }
    }
}