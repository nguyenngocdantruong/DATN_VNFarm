using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Claims;

namespace VNFarm_FinalFinal.Middlewares
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                context.Response.Redirect("/Home/Login");
                return;
            }

            var isUser = context.User.IsInRole("Buyer") || context.User.IsInRole("Seller");
            if (!isUser)
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Bạn không có quyền truy cập tài nguyên này.");
                return;
            }

            await _next(context);
        }
    }
} 