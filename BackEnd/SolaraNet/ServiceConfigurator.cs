using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Implementations;
using SolaraNet.BusinessLogic.Implementations.Validators;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
using SolaraNet.DataAccessLayer.EntityFramework.Repositories;

namespace SolaraNet
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
                .AddTransient<IUserValidator<DBUser>, SmartUserValidator<DBUser>>()
                .AddTransient<IIdentityUserService, IdentityUserService>()
                .AddIdentity<DBUser, IdentityRole>()
                .AddEntityFrameworkStores<SolaraNetDBContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;

                options.User.AllowedUserNameCharacters += GetRussianChars();
            });
            
        }

        private static List<char> GetRussianChars()
        {
            List<char> alphabet = new List<char>();
            for (int i = 1040; i < 1072; i++)
            {
                alphabet.Add((char)i);
                //добавляем Ё
                if (i == 1045)
                    alphabet.Add((char)1025);
            }

            return alphabet;
        }
    }
}