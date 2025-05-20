using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Claims;

namespace VNFarm.Middlewares
{
    public class SellerMiddleware
    {
        private readonly RequestDelegate _next;

        public SellerMiddleware(RequestDelegate next)
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

            var isSeller = context.User.IsInRole("Seller");
            if (!isSeller)
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Bạn không có quyền truy cập tài nguyên này.");
                return;
            }

            await _next(context);
        }
    }
} 