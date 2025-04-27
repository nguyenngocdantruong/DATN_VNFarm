using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace VNFarm.Middlewares
{
    public class LoginOnlyMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginOnlyMiddleware(RequestDelegate next)
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

            await _next(context);
        }
    }
} 