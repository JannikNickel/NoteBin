using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NoteBin.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestLoggingMiddleware> logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch sw = Stopwatch.StartNew();
            logger.LogInformation("[REQUEST] {} {}{}", context.Request.Method, context.Request.Host, context.Request.Path);
            await next(context);
            logger.LogInformation("[RESPONSE] {} ({:F1}ms)", context.Response.StatusCode, sw.Elapsed.TotalMilliseconds);
        }
    }
}