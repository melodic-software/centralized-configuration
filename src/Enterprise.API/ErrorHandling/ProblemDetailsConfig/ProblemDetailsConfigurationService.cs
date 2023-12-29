using Enterprise.API.ErrorHandling.Options;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.API.ErrorHandling.ProblemDetailsConfig
{
    public static class ProblemDetailsConfigurationService
    {
        internal static void ConfigureProblemDetails(this IServiceCollection services, WebApplicationBuilder builder, ErrorHandlingConfigurationOptions errorHandlingConfigOptions)
        {
            bool includeExceptionDetails = !builder.Environment.IsProduction();

            // This uses a middleware package that transforms exceptions to consistent problem responses based on RFC7807.
            // It uses a machine-readable format for specifying errors in an HTTP API.
            // https://datatracker.ietf.org/doc/html/rfc7807
            // https://github.com/khellang/Middleware/tree/25eac131b2595fa72e2072c87c24763e42bd8e31
            // https://github.com/khellang/Middleware/issues/149
            // https://andrewlock.net/handling-web-api-exceptions-with-problemdetails-middleware/
            services.AddProblemDetails(options =>
            {
                // Only show errors in non production environments.
                options.IncludeExceptionDetails = (httpContext, exception) => includeExceptionDetails;
                options.OnBeforeWriteDetails = (httpContext, problemDetails) =>
                {
                    // We want to obfuscate exception details to clients of the API.
                    if (problemDetails.Status == 500 && !includeExceptionDetails)
                        problemDetails.Detail = errorHandlingConfigOptions.InternalServerErrorResponseDetailMessage;
                };

                options.Rethrow<SqliteException>();
                options.Rethrow<SqlException>();
                //options.Rethrow<Exception>();

                // This is an application "fault", which is semantically different from an "error".
                options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            });
        }

        internal static void UseProblemDetails(this WebApplication app, ErrorHandlingConfigurationOptions errorHandlingConfigOptions)
        {
            app.UseProblemDetails();
        }
    }
}
