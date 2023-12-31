﻿using Enterprise.API.ErrorHandling.Options;
using Enterprise.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.API.ErrorHandling.ProblemDetailsMiddleware;

internal static class HellangMiddlewareService
{
    internal static void AddProblemDetails(IServiceCollection services, WebApplicationBuilder builder, ErrorHandlingConfigurationOptions errorHandlingConfigOptions)
    {
        // Only show exception details in non production environments.
        bool includeExceptionDetails = !builder.Environment.IsProduction();

        // This uses a middleware package that transforms exceptions to consistent problem responses based on RFC7807.
        // It uses a machine-readable format for specifying errors in an HTTP API.
        // https://datatracker.ietf.org/doc/html/rfc7807
        // https://github.com/khellang/Middleware/tree/25eac131b2595fa72e2072c87c24763e42bd8e31
        // https://github.com/khellang/Middleware/issues/149
        // https://andrewlock.net/handling-web-api-exceptions-with-problemdetails-middleware/
        services.AddProblemDetails(options =>
        {
            options.IsProblem = HellangMiddlewareDelegates.IsProblem;

            options.IncludeExceptionDetails = (httpContext, exception) =>
            {
                if (exception is NotFoundException or ValidationException)
                    return false;

                return includeExceptionDetails;
            };

            options.OnBeforeWriteDetails = (httpContext, problemDetails) =>
            {
                // We want to obfuscate exception details to clients of the API.
                if (problemDetails.Status == StatusCodes.Status500InternalServerError && !includeExceptionDetails)
                    problemDetails.Detail = errorHandlingConfigOptions.InternalServerErrorResponseDetailMessage;
            };

            // These are ignored by this middleware.
            // This is convenient if you want to pass through up to other exception middleware components
            //options.Ignore<T>();

            // NOTE: Not sure if this works with the current setup.
            options.Rethrow<SqliteException>();
            options.Rethrow<SqlException>();
            //options.Rethrow<ValidationException>();
            //options.Rethrow<Exception>();
            //options.RethrowAll();

            // TODO: Move to separate class.
            options.Map<ValidationException>(exception =>
            {
                Dictionary<string, string[]> errorDictionary = exception.ValidationErrors
                    .ToDictionary(x => x.PropertyName, x => new[] { x.ErrorMessage });

                ValidationProblemDetails problemDetails = new ValidationProblemDetails(errorDictionary)
                {
                    Status = StatusCodes.Status422UnprocessableEntity
                };

                return problemDetails;
            });

            // These are available for use, but are entirely optional.
            // These can be handled manually in controller / framework code OR exceptions can be raised and caught here.
            options.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
            options.MapToStatusCode<BadRequestException>(StatusCodes.Status400BadRequest);

            // This is an application "fault", which is semantically different from an "error".
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        });
    }

    internal static void UseProblemDetails(WebApplication app, ErrorHandlingConfigurationOptions errorHandlingConfigOptions)
    {
        app.UseProblemDetails();
    }
}