using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Implementations;
using SolaraNet.BusinessLogic.Implementations.Mapper;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
using SolaraNet.DataAccessLayer.EntityFramework.Repositories;
using SolaraNet.Mapper;
using SolaraNet.TelegramBot.Commands;

namespace SolaraNet.TelegramBot
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                
                .ConfigureServices((hostContext, services) =>
                 { 
                    
                    services.AddLogging();
                    services.AddSingleton<IChatService, TelegramService>();
                     services.AddTransient<ITelegramService, SolaraNet.BusinessLogic.Implementations.TelegramService>();
                    services.AddTransient<ITelegramRepository, TelegramRepository>();
                     DBContextInstaller.ConfigureDbContext(services);
                    services.AddBotCommands();
                    services.AddHostedService<Bot>();
                     services
                .AddSingleton<IMapper>(new AutoMapper.Mapper(GetMapperConfiguration()));
                 });
        private static MapperConfiguration GetMapperConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AdvertismentsMapper>();
                cfg.AddProfile<SimplePaginationMapper>();
            });
            configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
   
}
