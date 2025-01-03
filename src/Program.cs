using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NoteBin.Configuration;
using System.IO;

namespace NoteBin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            ConfigureBuilder(builder);
            ConfigureService(builder.Services, builder.Configuration);

            WebApplication app = builder.Build();
            ValidateConfiguration(app);
            ConfigureMiddleware(app);
            ConfigureEndpoints(app);
            app.Run();
        }

        private static void ConfigureBuilder(WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
        }

        private static void ConfigureService(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddNoteStorageServices(configuration);
            services.AddUserStorageServices(configuration);
            services.AddAuthServices(configuration);
            services.AddControllers();
        }

        private static void ValidateConfiguration(WebApplication app)
        {
            NoteStorageConfiguration.Initialize(app);
            UserStorageConfiguration.Initialize(app);
            AuthConfiguration.Initialize(app);
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            if(app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.Map("/error", _ => Results.InternalServerError());
            }

            app.UseRequestLogging();

            string path = Path.GetFullPath("web");//TODO this should be part of the config
            PhysicalFileProvider webPath = new PhysicalFileProvider(path);
            app.Environment.WebRootFileProvider = webPath;

            //Serve static files, if not requesting API or existing static files
            app.Use(async (HttpContext context, RequestDelegate next) =>
            {
                HttpRequest request = context.Request;
                bool isApi = request.Path.StartsWithSegments("/api");
                if(!isApi && !webPath.GetFileInfo(request.Path.Value?.TrimStart('/') ?? "").Exists)
                {
                    request.Path = "/";
                }
                await next(context);
            });
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = webPath
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = webPath
            });
        }

        private static void ConfigureEndpoints(WebApplication app)
        {
            app.MapControllers();
        }
    }
}