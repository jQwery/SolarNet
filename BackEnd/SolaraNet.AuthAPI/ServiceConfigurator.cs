using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Implementations;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
using SolaraNet.DataAccessLayer.EntityFramework.Repositories;

namespace SolaraNet.AuthAPI
{
    public class ServiceConfigurator
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAdvertismentService, AdvertismentService>()
#pragma warning disable 618
                .AddTransient<IUserService, UserService>()
#pragma warning restore 618
                .AddTransient<IAdvertismentRepository, AdvertismentRepository>()
#pragma warning disable 618
                .AddTransient<IUserRepository, UserRepository>()
#pragma warning restore 618
                .AddTransient<ISaver, Saver>()
                .AddTransient<IEmailService, EmailService>()
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IIdentityUserService, IdentityUserService>()
                .AddIdentity<DBUser, IdentityRole>()
                .AddEntityFrameworkStores<SolaraNetDBContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            //services.AddScoped<IIdentityService, IdentityService>();
        }
    }
}