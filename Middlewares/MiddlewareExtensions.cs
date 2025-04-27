using Microsoft.AspNetCore.Builder;

namespace VNFarm.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginOnly(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginOnlyMiddleware>();
        }

        public static IApplicationBuilder UseAdminOnly(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminMiddleware>();
        }

        public static IApplicationBuilder UseUserOnly(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserMiddleware>();
        }
    }
} 