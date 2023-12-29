using Enterprise.API.ErrorHandling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enterprise.API.ErrorHandling.Middleware;

// NOTE: This is an alternate approach to global error handling.
// Take a look at the ConfigureGlobalErrorHandler method in the ErrorHandlingConfigurationService.

[Obsolete("Use IExceptionHandler instead of middleware. This was introduced with .NET 8.")]
public class GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
{
    private readonly ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        // One of the advantages of using this middleware over "app.UseExceptionHandler"
        // is that we can perform pre- and post-handling steps (even if there isn't an error to handle).

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        await GlobalErrorHandler.HandleError(context, exception, _logger);
    }
}