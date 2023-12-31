﻿using System.Net.Mime;
using System.Text.Json;
using Enterprise.API.ErrorHandling.Model;
using Enterprise.Exceptions;
using Enterprise.Logging.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Enterprise.API.ErrorHandling.Shared;

[Obsolete("Use IExceptionHandler instead of middleware. This was introduced with .NET 8.")]
public class GlobalErrorHandler
{
    internal const string ErrorMessage = "Something went wrong.";

    internal static async Task HandleError(HttpContext context, Exception exception, ILogger? logger)
    {
        if (context.Response.HasStarted)
            return;

        // These are two separate methods for handling the error.
        // Since exception handlers or the Hellang middleware is more likely to be used I left this as is.

        //await HandleError(context, exception, logger);
        await HandleProblemDetails(context, exception);
    }

    private static async Task UseSimpleError(HttpContext context, Exception exception, ILogger logger)
    {
        // TODO: Provide functionality to use JSON or XML depending on the request header.

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        JsonSerializerDefaults serializationDefaults = JsonSerializerDefaults.Web;
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(serializationDefaults);

        // This is an optional numeric ID and name that can be used when logging that represents the "type" of event.
        EventId eventId;

        ErrorDetailsDto errorDetailsDto = new ErrorDetailsDto(context.Response.StatusCode, ErrorMessage);

        string json = JsonSerializer.Serialize(errorDetailsDto, jsonSerializerOptions);

        if (exception is HttpRequestException httpRequestException)
        {
            if (httpRequestException.StatusCode.HasValue)
                context.Response.StatusCode = (int)httpRequestException.StatusCode.Value;

            // TODO: The result objects / exceptions might have additional properties.

            await context.Response.WriteAsync(json);

            eventId = LogEventIds.CustomError;
        }
        else
        {
            await context.Response.WriteAsync(json);

            eventId = LogEventIds.UnknownError;
        }

        logger?.LogError(eventId, exception, exception.Message);
    }

    private static async Task HandleProblemDetails(HttpContext context, Exception exception)
    {
        ExceptionDetails exceptionDetails = GetExceptionDetails(exception);

        ProblemDetails problemDetails = new ProblemDetails
        {
            Status = exceptionDetails.Status,
            Type = exceptionDetails.Type,
            Title = exceptionDetails.Title,
            Detail = exceptionDetails.Detail
        };

        if (exceptionDetails.Errors is not null)
            problemDetails.Extensions["errors"] = exceptionDetails.Errors;

        context.Response.StatusCode = exceptionDetails.Status;

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "ValidationFailure",
                "Validation Error",
                "One or more validation errors has occurred.",
                validationException.ValidationErrors),
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "ServerError",
                "Server Error",
                "An unexpected error has occurred.",
                null)
        };
    }
}

internal record ExceptionDetails
{
    public ExceptionDetails(int Status, string Type, string Title, string Detail, IEnumerable<object>? Errors)
    {
        this.Status = Status;
        this.Type = Type;
        this.Title = Title;
        this.Detail = Detail;
        this.Errors = Errors;
    }

    public int Status { get; init; }
    public string Type { get; init; }
    public string Title { get; init; }
    public string Detail { get; init; }
    public IEnumerable<object>? Errors { get; init; }

    public void Deconstruct(out int Status, out string Type, out string Title, out string Detail, out IEnumerable<object>? Errors)
    {
        Status = this.Status;
        Type = this.Type;
        Title = this.Title;
        Detail = this.Detail;
        Errors = this.Errors;
    }
}