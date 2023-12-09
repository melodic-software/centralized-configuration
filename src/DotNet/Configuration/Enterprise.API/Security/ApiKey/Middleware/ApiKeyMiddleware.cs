using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using static Enterprise.API.Security.ApiKey.ApiKeyService;
using static Enterprise.API.Swagger.Services.SwaggerRequestDetectionService;

// use EITHER the attribute OR this middleware
// this middleware could be extended to allow for multiple keys (one for each external party)
// all keys could exist in configuration, or a database

namespace Enterprise.API.Security.ApiKey.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        if (RequestContainsValidApiKey(context, configuration))
        {
            await _next(context);
            return;
        }

        if (SwaggerPageRequested(context))
        {
            await _next(context);
            return;
        }

        // pipeline is terminated here
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Invalid API Key");
    }
}