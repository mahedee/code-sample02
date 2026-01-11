using System.Diagnostics;

namespace MiddlewareExample.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Log Request details
            Console.WriteLine($"Incoming Request: {context.Request.Method} {context.Request.Path}");

            await _next(context);

            stopwatch.Stop();

            // Log Response details
            Console.WriteLine($"Outgoing Response: {context.Response.StatusCode} | Time Taken: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
