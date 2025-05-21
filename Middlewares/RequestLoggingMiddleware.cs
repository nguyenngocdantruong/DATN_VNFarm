namespace VNFarm.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;
            if (!string.IsNullOrEmpty(path) && path.Contains("/api/", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    "[{Time}] {Method} {Path} from {IP}",
                    DateTime.Now.ToString("HH:mm:ss"),
                    context.Request.Method,
                    context.Request.Path,
                    context.Connection.RemoteIpAddress?.ToString()
                );
            }
            await _next(context);
        }
    }
}
