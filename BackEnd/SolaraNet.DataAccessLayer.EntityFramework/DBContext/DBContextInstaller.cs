using System;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.EntityFramework.Repositories;

namespace SolaraNet.DataAccessLayer.EntityFramework.DBContext
{
    public class DBContextInstaller
    {
        public static void ConfigureDbContext(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build(); 


            services
                .AddDbContext<SolaraNetDBContext>(o =>
                    o.UseLazyLoadingProxies().UseSqlServer(config.GetConnectionString("SolaraNetDb"), builder =>
                        builder.MigrationsAssembly(
                            typeof(DataAccessModel).Assembly.FullName)))
                .AddTransient<ISaver, Saver>();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));
        }
    }
}