using Microsoft.AspNetCore.Builder;
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
            services.AddControllers();
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

            string path = Path.Combine(app.Environment.ContentRootPath, "..", "web");
            PhysicalFileProvider webPath = new PhysicalFileProvider(path);
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