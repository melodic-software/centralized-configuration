using Enterprise.API.ErrorHandling.Options;
using Enterprise.API.ErrorHandling.ProblemDetailsMiddleware;
using Enterprise.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.ErrorHandling;

public static class ErrorHandlingConfigurationService
{
    public static void ConfigureErrorHandling(this IServiceCollection services, WebApplicationBuilder builder, ErrorHandlingConfigurationOptions errorHandlingConfigOptions)
    {
        // https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8
        // NOTE: This will not be run if the Hellang middleware is registered.
        //services.AddExceptionHandler<GlobalExceptionHandler>();
        
        services.AddProblemDetails(builder, errorHandlingConfigOptions);
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

        // We're relying on the Hellang problem details middleware as our global exception handler
        // This line can cause a runtime error if problem details service middleware (Microsoft default) is not registered
        app.UseExceptionHandler();

        app.UseProblemDetails(errorHandlingConfigOptions);
    }
}