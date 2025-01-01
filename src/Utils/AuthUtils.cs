using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace NoteBin
{
    public static class AuthUtils
    {
        private const string bearerStart = "Bearer ";

        public static string? ReadBearerToken(HttpRequest request)
        {
            StringValues header = request.Headers.Authorization;
            if(header.Count > 0)
            {
                string authHeader = header.ToString();
                if(authHeader.StartsWith(bearerStart))
                {
                    return authHeader[bearerStart.Length..].Trim();
                }
            }
            return null;
        }
    }
}