using Microsoft.AspNetCore.Builder;
using NoteBin.Middleware;

namespace NoteBin.Configuration
{
    public static class RequestLoggingConfiguration
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}