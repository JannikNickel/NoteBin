using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NoteBin.Services;
using System;

namespace NoteBin.Configuration
{
    public static class AuthConfiguration
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            ConfigurationHelper.AddValidatedSettings<AuthSettings>(services, configuration);

            services.AddSingleton<IAuthService>((IServiceProvider provider) =>
            {
                AuthSettings settings = provider.GetRequiredService<IOptions<AuthSettings>>().Value;
                return settings.AuthType switch
                {
                    AuthType.Stateless => new StatelessAuthService(settings),
                    _ => throw new NotSupportedException($"{nameof(AuthType)} '{settings.AuthType}' is not supported!")
                };
            });
            return services;
        }

        public static void Initialize(WebApplication app)
        {
            //Make sure service initialization works
            _ = app.Services.GetRequiredService<IAuthService>();
        }
    }
}