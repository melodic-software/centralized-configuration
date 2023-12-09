using Enterprise.API.Cors.Constants;
using Enterprise.API.Cors.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.API.Cors;

public static class CorsConfigurationService
{
    public static void ConfigureCors(this IServiceCollection services, IWebHostEnvironment environment, CorsConfigurationOptions options)
    {
        if (!options.EnableCors)
            return;

        if (options.ConfigureCustom != null)
        {
            // allow for full customization
            services.AddCors(options.ConfigureCustom);
            return;
        }

        if (!environment.IsProduction())
        {
            // this should only be used in pre production environments
            services.AddCors(RelaxedCorsConfiguration);
            return;
        }

        string policyName = CorsConfigurationConstants.DefaultPolicyName;
        string[] allowedOrigins = options.AllowedOrigins.ToArray();

        // TODO: add logging (warning) here instead of throwing an exception
        // the logger may not be available until the application is built, so a collection of log messages may need to be instantiated
        // or a logger reference may need to be instantiated elsewhere and referenced here
        if (!allowedOrigins.Any())
            throw new Exception("CORS has been enabled, but no origins are allowed.");

        services.AddCors(corsOptions =>
        {
            StandardCorsConfiguration(corsOptions, policyName, allowedOrigins);
        });
    }

    public static void UseCors(this WebApplication app, CorsConfigurationOptions corsConfigOptions)
    {
        if (corsConfigOptions.EnableCors)
            app.UseCors(CorsConfigurationConstants.DefaultPolicyName);
    }

    /// <summary>
    /// This allows any origin, HTTP method or header. It should not be used in production environments.
    /// </summary>
    public static Action<CorsOptions> RelaxedCorsConfiguration => options =>
    {
        string policyName = CorsConfigurationConstants.RelaxedPolicyName;

        if (CorsPolicyService.PolicyExists(options, policyName))
            return;

        options.AddPolicy(policyName, corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    };

    public static Action<CorsOptions, string, string[]> StandardCorsConfiguration => (options, policyName, allowedOrigins) =>
    {
        if (CorsPolicyService.PolicyExists(options, policyName))
            return;

        options.AddPolicy(policyName, corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    };
}