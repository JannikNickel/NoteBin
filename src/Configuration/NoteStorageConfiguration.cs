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
            if(!configuration.GetSection(NoteStorageSettings.SectionName).Exists())
            {
                throw new InvalidOperationException($"Configuration section '{NoteStorageSettings.SectionName}' is missing!");
            }

            services.AddOptions<NoteStorageSettings>()
                .BindConfiguration(NoteStorageSettings.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton<INoteIdGenService, UuidNoteIdGenService>();
            services.AddSingleton<INoteContentService>((IServiceProvider provider) =>
            {
                NoteStorageSettings settings = provider.GetRequiredService<IOptions<NoteStorageSettings>>().Value;
                return settings.StorageType switch
                {
                    NoteStorageType.Memory => new MemoryNoteContentService(),
                    NoteStorageType.SQLite => new FileNoteContentService(settings.ContentPath!),
                    _ => throw new NotSupportedException($"NoteStorageType '{settings.StorageType}' is not supported!")
                };
            });
            services.AddSingleton<INoteDbService>((IServiceProvider provider) =>
            {
                NoteStorageSettings settings = provider.GetRequiredService<IOptions<NoteStorageSettings>>().Value;
                INoteIdGenService idGenService = provider.GetRequiredService<INoteIdGenService>();
                INoteContentService contentService = provider.GetRequiredService<INoteContentService>();

                return settings.StorageType switch
                {
                    NoteStorageType.Memory => new MemoryNoteDbService(idGenService, contentService),
                    NoteStorageType.SQLite => new SqLiteNoteDbService(idGenService, contentService, settings.ConnectionString),
                    _ => throw new NotSupportedException($"NoteStorageType '{settings.StorageType}' is not supported!")
                };
            });
            return services;
        }

        public static void Initialize(WebApplication app)
        {
            //Make sure service initialization works
            _ = app.Services.GetRequiredService<INoteDbService>();
        }
    }
}