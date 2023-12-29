using Enterprise.API.ErrorHandling.ExceptionHandlers;
using Enterprise.API.ErrorHandling.Middleware;
using Enterprise.API.ErrorHandling.Options;
using Enterprise.API.ErrorHandling.ProblemDetailsConfig;
using Enterprise.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.ErrorHandling;

public static class ErrorHandlingConfigurationService
{
    public static void ConfigureErrorHandling(this IServiceCollection services, WebApplicationBuilder builder, ErrorHandlingConfigurationOptions errorHandlingConfigOptions)
    {
        services.ConfigureProblemDetails(builder, errorHandlingConfigOptions);

        services.AddExceptionHandler<TimeOutExceptionHandler>();
        services.AddExceptionHandler<DefaultExceptionHandler>();
    }
    
    public static void UseErrorHandling(this WebApplication app, ErrorHandlingConfigurationOptions errorHandlingConfigOptions)
    {
        if (app.Environment.IsLocal() || app.Environment.IsDevelopment())
        {
            // TODO: Do we need to remove this if the custom global error handling middleware is being used?
            app.UseDeveloperExceptionPage();
        }
        else
        {

        }

        app.UseMiddleware<CriticalExceptionMiddleware>();
        app.UseProblemDetails(errorHandlingConfigOptions);

        app.UseExceptionHandler(opt => { });

        app.MapGet("/exceptions", () => {
            throw new NotImplementedException();
        });

        app.MapGet("/timeouts", () => {
            throw new TimeoutException();
        });
    }
}