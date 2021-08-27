using Microsoft.AspNetCore.Http;

namespace Veterinary.Api.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static string GetApplicationUrl(this IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{request.PathBase}";
        }
    }
}
