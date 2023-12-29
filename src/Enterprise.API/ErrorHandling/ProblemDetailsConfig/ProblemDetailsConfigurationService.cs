

using Enterprise.API.ErrorHandling.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.ErrorHandling.ProblemDetailsConfig
{
    public static class ProblemDetailsConfigurationService
    {
        internal static void AddProblemDetails(this IServiceCollection services, WebApplicationBuilder builder, ErrorHandlingConfigurationOptions errorHandlingConfigOptions)
        {
            // The default out of the box Microsoft "ProblemDetails" registrations.
            services.AddProblemDetails();

            // This configures the problem details using the "Hellang" middleware.
            HellangMiddlewareConfigService.ConfigureProblemDetails(services, builder, errorHandlingConfigOptions);
        }

        internal static void UseProblemDetails(this WebApplication app, ErrorHandlingConfigurationOptions errorHandlingConfigOptions)
        {
            HellangMiddlewareConfigService.UseProblemDetails(app, errorHandlingConfigOptions);
        }
    }
}
