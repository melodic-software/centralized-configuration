using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Enterprise.Logging.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enterprise.API.ErrorHandling.Shared;

public class GlobalErrorHandler
{
    internal const string ErrorMessage = "Something went wrong";

    internal static async Task HandleError(HttpContext context, Exception exception, ILogger? logger)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // TODO: handle this as JSON or XML depending on the request header
        context.Response.ContentType = MediaTypeNames.Application.Json;

        JsonSerializerDefaults serializationDefaults = JsonSerializerDefaults.Web;
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(serializationDefaults);

        // an optional numeric ID and name used when logging that represents the "type" of event
        EventId eventId; 

        if (exception is HttpRequestException httpRequestException)
        {
            if (httpRequestException.StatusCode.HasValue)
                context.Response.StatusCode = (int)httpRequestException.StatusCode.Value;

            // TODO: the result objects / exceptions might have additional properties

            string json = JsonSerializer.Serialize(new
            {
                context.Response.StatusCode,
                Message = ErrorMessage
            }, jsonSerializerOptions);

            await context.Response.WriteAsync(json);

            eventId = LogEventIds.CustomError;
        }
        else
        {
            string json = JsonSerializer.Serialize(new
            {
                context.Response.StatusCode,
                Message = ErrorMessage
            }, jsonSerializerOptions);

            await context.Response.WriteAsync(json);
             
            eventId = LogEventIds.UnknownError;
        }

        logger?.LogError(eventId, exception, exception.Message);
    }
}