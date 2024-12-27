using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NoteBin.Services;
using System;

namespace NoteBin.Configuration
{
    public static class NoteStorageConfiguration
    {
        public static IServiceCollection AddNoteStorageServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<NoteStorageSettings>(configuration.GetSection(NoteStorageSettings.SectionName));
            services.AddSingleton<INoteIdGenService, UuidNoteIdGenService>();
            services.AddSingleton<INoteDbService>((IServiceProvider provider) =>
            {
                NoteStorageSettings settings = provider.GetRequiredService<IOptions<NoteStorageSettings>>().Value;
                INoteIdGenService idGenService = provider.GetRequiredService<INoteIdGenService>();

                return settings.StorageType switch
                {
                    NoteStorageType.Memory => new MemoryNoteDbService(idGenService),
                    NoteStorageType.SQLite => new SqLiteNoteDbService(idGenService, settings.ConnectionString),
                    _ => throw new NotSupportedException($"NoteStorageType {settings.StorageType} is not supported!")
                };
            });
            return services;
        }
    }
}