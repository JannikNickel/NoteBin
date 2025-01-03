using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace NoteBin
{
    public static class AuthHelper
    {
        private const string BearerStart = "Bearer ";

        public static string? ReadBearerToken(HttpRequest request)
        {
            StringValues header = request.Headers.Authorization;
            if(header.Count > 0)
            {
                string authHeader = header.ToString();
                if(authHeader.StartsWith(BearerStart))
                {
                    return authHeader[BearerStart.Length..].Trim();
                }
            }
            return null;
        }
    }
}