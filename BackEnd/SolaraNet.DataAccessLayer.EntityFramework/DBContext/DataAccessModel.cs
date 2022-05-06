using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.EntityFramework.Repositories;

namespace SolaraNet.DataAccessLayer.EntityFramework.DBContext
{
    public static class DataAccessModel
    {
        public sealed class ModuleConfiguration
        {
            public IServiceCollection Services { get; set; }
        }

        public static IServiceCollection AddDataAccessModule(
            this IServiceCollection services,
            Action<ModuleConfiguration> action
        )
        {
            var moduleConfiguration = new ModuleConfiguration
            {
                Services = services
            };
            action(moduleConfiguration);
            return services;
        }

    }
}