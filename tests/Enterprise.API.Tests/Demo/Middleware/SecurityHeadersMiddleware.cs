using Microsoft.AspNetCore.Http;

namespace Enterprise.API.Tests.Demo.Middleware;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        IHeaderDictionary headers = context.Request.Headers;

        // add CSP + X-Content-Type
        headers["Content-Security-Policy"] = "default-src 'self';frame-ancestors 'none';";
        headers["X-Content-Type-Options"] = "nosniff";

        await _next(context);
    }
}