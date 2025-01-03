using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace NoteBin.Configuration
{
    public static class StaticFileConfiguration
    {
        public static IApplicationBuilder UseStaticFileServing(this WebApplication app)
        {
            string path = Path.GetFullPath(Constants.WebDir);
            PhysicalFileProvider webPath = new PhysicalFileProvider(path);
            app.Environment.WebRootFileProvider = webPath;

            //Serve static files, if not requesting API or existing static files
            app.Use(async (HttpContext context, RequestDelegate next) =>
            {
                HttpRequest request = context.Request;
                bool isApi = request.Path.StartsWithSegments(Constants.ApiPrefix);
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

            return app;
        }
    }
}