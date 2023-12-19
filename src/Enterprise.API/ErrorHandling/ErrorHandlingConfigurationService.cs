using Enterprise.API.ErrorHandling.Constants;
using Enterprise.API.ErrorHandling.Middleware;
using Enterprise.API.ErrorHandling.Options;
using Enterprise.API.ErrorHandling.Shared;
using Enterprise.API.Routing;
using Enterprise.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.API.ErrorHandling;

public static class ErrorHandlingConfigurationService
{
    public static void ConfigureErrorHandling(this WebApplication app, ErrorHandlingConfigurationOptions errorHandlingConfigOptions)
    {
        if (app.Environment.IsLocal() || app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {

        }

        app.UseMiddleware<CriticalExceptionMiddleware>();
        app.UseProblemDetails(errorHandlingConfigOptions);
    }

    private static void UseDevelopmentErrorHandler(this WebApplication app)
    {
        app.UseExceptionHandler(RouteTemplates.DevelopmentErrorHandler);
    }

    private static void UserProductionErrorHandler(this WebApplication app)
    {
        app.UseExceptionHandler(RouteTemplates.ErrorHandler);
    }

    private static void ConfigureGlobalErrorHandler(this WebApplication app, ILogger? logger)
    {
        // NOTE: this is one approach to global error handling
        // one alternative is to use a middleware component - take a look at CustomErrorMiddleware.cs

        // the only difference here is that the following will only be called when an unhandled exception occurs
        // the middleware allows for pre and post processing of the exception (even if there isn't one to be handled)

        app.UseExceptionHandler(appBuilder =>
        {
            // this is a global exception handler
            // if it reaches here, it hasn't been handled yet and likely hasn't been logged

            appBuilder.Run(async context =>
            {
                IExceptionHandlerFeature? contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    Exception exception = contextFeature.Error;

                    // here we can handle specific exception types and return more explicit status codes
                    // TODO: add delegate hook for handling additional exception types OR override this completely
                    // maybe use a dictionary for handle

                    await GlobalErrorHandler.HandleError(context, exception, logger);
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(ErrorConstants.GenericErrorMessage);
                }
            });
        });
    }
}