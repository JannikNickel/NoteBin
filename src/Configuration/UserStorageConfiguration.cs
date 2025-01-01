using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NoteBin.Services;
using System;

namespace NoteBin.Configuration
{
    public static class UserStorageConfiguration
    {
        public static IServiceCollection AddUserStorageServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            ConfigurationHelper.AddValidatedSettings<UserStorageSettings>(services, configuration);

            services.AddSingleton<IUserDbService>((IServiceProvider provider) =>
            {
                UserStorageSettings settings = provider.GetRequiredService<IOptions<UserStorageSettings>>().Value;

                return settings.StorageType switch
                {
                    UserStorageType.Memory => new MemoryUserDbService(),
                    UserStorageType.SQLite => new SqLiteUserDbService(settings.ConnectionString),
                    _ => throw new NotSupportedException($"{nameof(UserStorageType)} '{settings.StorageType}' is not supported!")
                };
            });
            return services;
        }

        public static void Initialize(WebApplication app)
        {
            //Make sure service initialization works
            _ = app.Services.GetRequiredService<IUserDbService>();
        }
    }
}