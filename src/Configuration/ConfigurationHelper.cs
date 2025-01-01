using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace NoteBin.Configuration
{
    public static class ConfigurationHelper
    {
        public static void AddValidatedSettings<T>(IServiceCollection services, ConfigurationManager configuration) where T : class, ISettingsObject
        {
            if(!configuration.GetSection(T.SectionName).Exists())
            {
                throw new InvalidOperationException($"Configuration section '{T.SectionName}' is missing!");
            }

            services.AddOptions<T>()
                .BindConfiguration(T.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}