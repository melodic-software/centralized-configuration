using Enterprise.Exceptions;
using Enterprise.Serialization.Json;
using Enterprise.Validation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace Enterprise.API.ErrorHandling.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
            return false;

        int statusCode = StatusCodes.Status422UnprocessableEntity;

        httpContext.Response.StatusCode = statusCode;

        List<ValidationError> validationErrors = validationException.ValidationErrors.ToList();

        if (!validationErrors.Any())
            return true;

        Dictionary<string, string> dictionary = validationErrors
            .ToDictionary(x => x.PropertyName, x => x.ErrorMessage);

        StringValues acceptHeader = httpContext.Request.Headers.Accept;

        bool mediaTypeParsed = MediaTypeHeaderValue.TryParse(acceptHeader.ToString(), out MediaTypeHeaderValue? parsedMediaType);

        // TODO: Use constants instead of magic strings.
        bool useJson = !mediaTypeParsed || parsedMediaType?.Type == "*" || parsedMediaType?.Type == "json";

        if (useJson)
        {
            await CreateJsonResponse(httpContext, cancellationToken, dictionary);
        }
        else
        {
            // TODO: Serialize to XML and set Content-Type appropriately (application/xml)
            // For now, we're just going to return JSON.
            await CreateJsonResponse(httpContext, cancellationToken, dictionary);
        }

        return true;
    }

    private static async Task CreateJsonResponse(HttpContext httpContext, CancellationToken cancellationToken, Dictionary<string, string> dictionary)
    {
        JsonSerializerOptions serializerOptions = JsonSerializerOptionsService.GetDefaultOptions();
        await httpContext.Response.WriteAsJsonAsync(dictionary, serializerOptions, cancellationToken: cancellationToken);
    }
}