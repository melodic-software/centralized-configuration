using Enterprise.Testing.Services;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Enterprise.API.Tests.Demo.Middleware.Tests;

public class SecurityHeadersMiddlewareTest
{
    [Fact]
    public async Task InvokeAsync_Invoke_SetsExpectedResponseHeaders()
    {
        // ARRANGE
        HttpContext httpContext = HttpContextCreationService.CreateDefaultContext();
        RequestDelegate next = _ => Task.CompletedTask;

        SecurityHeadersMiddleware middleware = new SecurityHeadersMiddleware(next);

        // ACT
        await middleware.InvokeAsync(httpContext);

        // ASSERT
        string cspHeader = httpContext.Request.Headers["Content-Security-Policy"].ToString();
        string xContentTypeOptionsHeader = httpContext.Request.Headers["X-Content-Type-Options"].ToString();

        Assert.Equal("default-src 'self';frame-ancestors 'none';", cspHeader);
        Assert.Equal("nosniff", xContentTypeOptionsHeader);
    }
}