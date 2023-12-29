using System.Net.Mime;
using System.Text.Json;
using Enterprise.API.ErrorHandling.Model;
using Enterprise.Logging.Events;
using Microsoft.AspNetCore.Http;
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
}