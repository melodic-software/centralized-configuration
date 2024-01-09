using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Collections.Concurrent;

namespace Enterprise.API.Controllers.Formatters.Output.Custom;

/// <summary>
/// The API can return a 406 status code if the MvcOptions property ReturnHttpNotAcceptable is set to true, and a media type version strategy is used.
/// When enabled, this can cause problems with generated Swagger documentation. Consider setting it to false and relying on this formatter to handle the 406 responses.
/// This output formatter compensates by removing any versioning parameter and delegating the modified context to the existing formatters.
/// This should be registered last so that all dynamically added output formatters can be referenced.
/// If there are no formatters than can handle the media type, a 406 response is generated.
/// </summary>
public class MediaTypeVersionDelegatingFormatter : IOutputFormatter
{
    private readonly IList<IOutputFormatter> _formatters;
    private readonly ConcurrentDictionary<StringSegment, IOutputFormatter> _formatterCache = new();
    private readonly string _versioningParameterName;

    /// <summary>
    /// The API can return a 406 status code if the MvcOptions property ReturnHttpNotAcceptable is set to true, and a media type version strategy is used.
    /// When enabled, this can cause problems with generated Swagger documentation. Consider setting it to false and relying on this formatter to handle the 406 responses.
    /// This output formatter compensates by removing any versioning parameter and delegating the modified context to the existing formatters.
    /// This should be registered last so that all dynamically added output formatters can be referenced.
    /// If there are no formatters than can handle the media type, a 406 response is generated.
    /// </summary>
    /// <param name="formatters"></param>
    /// <param name="versioningParameterName"></param>
    public MediaTypeVersionDelegatingFormatter(IList<IOutputFormatter> formatters, string versioningParameterName)
    {
        _versioningParameterName = versioningParameterName;
        _formatters = formatters ?? throw new ArgumentNullException(nameof(formatters));
    }

    public bool CanWriteResult(OutputFormatterCanWriteContext context)
    {
        MediaTypeHeaderValue? mediaTypeHeaderValueWithoutVersion = GetMediaTypeHeaderValueWithoutVersion(context);
        
        if (mediaTypeHeaderValueWithoutVersion == null)
            return false;

        IOutputFormatter? formatter = GetFormatter(context, mediaTypeHeaderValueWithoutVersion);

        if (formatter == null)
            return false;
        
        // Cache the formatter for reuse in WriteAsync.
        _formatterCache[context.ContentType] = formatter;
        
        return true;
    }

    public async Task WriteAsync(OutputFormatterWriteContext context)
    {
        if (_formatterCache.TryGetValue(context.ContentType, out IOutputFormatter? formatter))
        {
            await formatter.WriteAsync(context);
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status406NotAcceptable;
        }
    }

    private MediaTypeHeaderValue? GetMediaTypeHeaderValueWithoutVersion(OutputFormatterCanWriteContext context)
    {
        IList<MediaTypeHeaderValue> acceptHeader = context.HttpContext.Request.GetTypedHeaders().Accept;

        if (!acceptHeader.Any())
            return null;

        foreach (MediaTypeHeaderValue headerValue in acceptHeader)
        {
            NameValueHeaderValue? versionParameter = headerValue.Parameters
                .FirstOrDefault(x => x.Name == _versioningParameterName);

            if (versionParameter != null)
                headerValue.Parameters.Remove(versionParameter);

            return headerValue;
        }

        return null;
    }

    private IOutputFormatter? GetFormatter(OutputFormatterCanWriteContext context, MediaTypeHeaderValue mediaTypeWithoutVersion)
    {
        // We only need the media type (without parameters) for the "ContentType".
        // If we wanted to preserve the original with parameters, use the full media type value.
        string? simpleMediaTypeValue = mediaTypeWithoutVersion.MediaType.Value;
        string fullMediaTypeValue = mediaTypeWithoutVersion.ToString();
        
        context.ContentType = new StringSegment(simpleMediaTypeValue);
        context.ContentTypeIsServerDefined = true;

        // Find a formatter that can handle the base media type.
        IOutputFormatter? formatter = _formatters.FirstOrDefault(f => f.CanWriteResult(context));

        return formatter;
    }
}