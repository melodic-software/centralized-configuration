using Enterprise.API.ErrorHandling.Constants;
using Enterprise.API.ErrorHandling.Shared;
using Enterprise.API.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;

namespace Enterprise.API.ErrorHandling.Middleware
{
    internal static class ExceptionHandlerMiddlewareService
    {
        [Obsolete("Use IExceptionHandler instead of middleware. This was introduced with .NET 8.")]
        public static void UseDevelopmentErrorHandler(this WebApplication app)
        {
            app.UseExceptionHandler(RouteTemplates.DevelopmentErrorHandler);
        }

        [Obsolete("Use IExceptionHandler instead of middleware. This was introduced with .NET 8.")]
        public static void UserProductionErrorHandler(this WebApplication app)
        {
            app.UseExceptionHandler(RouteTemplates.ErrorHandler);
        }

        [Obsolete("Use IExceptionHandler instead of middleware. This was introduced with .NET 8.")]
        public static void UseGlobalErrorHandler(this WebApplication app, ILogger? logger)
        {
            // NOTE: This is one approach to global error handling.
            // One alternative is to use a middleware component. Take a look at GlobalErrorHandlingMiddleware.cs.

            // The only difference here is that the following will only be called when an unhandled exception occurs.
            // The middleware allows for pre- and post-processing of the exception (even if there isn't one to be handled).

            app.UseExceptionHandler(appBuilder =>
            {
                // This is a global exception handler.
                // If it reaches here, it hasn't been handled yet and likely hasn't been logged.

                // This adds a terminal middleware delegate to the application pipeline.
                appBuilder.Run(async context =>
                {
                    IExceptionHandlerFeature? contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        string path = contextFeature.Path;
                        Endpoint? endpoint = contextFeature.Endpoint;
                        Exception exception = contextFeature.Error;
                        RouteValueDictionary routeValues = contextFeature.RouteValues ?? new RouteValueDictionary();

                        // Here we can handle specific exception types and return more explicit status codes.
                        // TODO: Add delegate hook for handling additional exception types OR override this completely.
                        // Maybe use a dictionary for handle?

                        await GlobalErrorHandler.HandleError(context, exception, logger);
                    }
                    else
                    {
                        // We don't have context of an exception, so we can only provide a generic response.
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync(ErrorConstants.GenericErrorMessage);
                    }
                });
            });
        }
    }
}
