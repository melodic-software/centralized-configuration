using Enterprise.API.Caching;
using Enterprise.API.Cors;
using Enterprise.API.Cors.Options;
using Enterprise.API.ErrorHandling;
using Enterprise.API.ErrorHandling.Options;
using Enterprise.API.Middleware;
using Enterprise.API.Options;
using Enterprise.API.Security;
using Enterprise.API.Swagger;
using Enterprise.API.Swagger.Constants;
using Enterprise.API.Swagger.Options;
using Enterprise.Hosting.Extensions;
using Enterprise.Logging;
using Enterprise.Logging.Options;
using Enterprise.Middleware;
using Enterprise.Monitoring.Health;
using Enterprise.Monitoring.Health.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;

namespace Enterprise.API;

public static class WebApplicationExtensions
{
    public static void ConfigureRequestPipeline(this WebApplication app, WebApplicationBuilder builder, ApiConfigurationOptions apiConfigOptions)
    {
        LoggingConfigurationOptions loggingConfigOptions = apiConfigOptions.LoggingConfigurationOptions;
        ErrorHandlingConfigurationOptions errorHandlingConfigOptions = apiConfigOptions.ErrorHandlingConfigurationOptions;
        HealthCheckOptions healthCheckOptions = apiConfigOptions.HealthCheckOptions;
        CorsConfigurationOptions corsConfigOptions = apiConfigOptions.CorsConfigurationOptions;
        SwaggerConfigurationOptions swaggerConfigOptions = apiConfigOptions.SwaggerConfigurationOptions;
        HttpRequestMiddlewareOptions options = apiConfigOptions.HttpRequestMiddlewareOptions;

        // the order in which we add request middleware is important
        // this is a chain of middleware that passes the request and response in and back out of the configured pipeline

        app.UseLogging(loggingConfigOptions);

        app.ConfigureErrorHandling(errorHandlingConfigOptions);

        // this will forward proxy headers to the current request
        // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-7.0
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        // we can also scope these to specific environments
        if (app.Environment.IsLocal() || app.Environment.IsDevelopment())
        {
            // shows all services registered with the container
            // TODO: is there a way to do this with compile-time safety? (constructor parameters)
            app.UseMiddleware<ListStartupServicesMiddleware>(builder.Services);
            app.UseMiddleware<RootRedirectMiddleware>(SwaggerConstants.RoutePrefix);
            app.UseSwagger(swaggerConfigOptions);
        }
        else
        {
            // this is a security feature that indicates to clients that HTTPs should be used for all future requests
            // the "Strict-Transport-Security" header is added
            // the default HSTS value is 30 days - you may want to change this for production scenarios
            // see RFC 6797
            app.UseHsts();
        }
           
        if (!app.Environment.IsLocal())
            app.UseHttpsRedirection();

        // enables using static files for the request
        // if a path is not set, it will use a wwwroot folder in the project by default
        // this also enables the addition of custom swagger UI stylesheets
        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors(corsConfigOptions);

        app.UseCaching();

        app.UseSecurity(app.Environment);

        app.UseHealthChecks(healthCheckOptions);

        // https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0#afwma
        //app.UseAntiforgery();

        // can be added via attribute or via "WithRequestTimeout()" for minimal APIs
        app.UseRequestTimeouts();

        // custom middleware registration
        options.AddCustomMiddleware?.Invoke(app);

        // this shortcut mixes request pipeline setup with route management
        // no routes are specified, and it is assumed attributes will be added to controllers and actions
        IEndpointConventionBuilder endpointConventionBuilder = app.MapControllers();

        // this locks down all controllers / endpoints
        // used the [AllowAnonymous] attribute on a controller or endpoint that needs to be publicly accessible
        // this impacts response caching, as GET/HEAD requests cannot be cached if an auth header is present in the request
        // NOTE: an alternative is to register the "AuthorizeFilter" globally in the controller configuration
        //endpointConventionBuilder.RequireAuthorization();
    }
}