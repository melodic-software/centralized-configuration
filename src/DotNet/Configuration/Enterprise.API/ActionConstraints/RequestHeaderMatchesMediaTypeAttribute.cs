using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using static Enterprise.API.Constants.HttpHeaderValueConstants;

namespace Enterprise.API.ActionConstraints;

/// <summary>
/// The API controller action will only execute when the specified media type(s) are present in the "Content-Type" request header.
/// This action constraint deals with routing, and doesn't have anything to do with input or output formatters.
/// Be sure to apply the [Consumes] attribute to the same controller action method as it restricts what an action can consume.
/// The [Consumes] attribute has to do with the media types for the input formatter.
/// The [Produces] attribute simply states that the media type can be produced on output (does not impact routing).
/// </summary>
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class RequestHeaderMatchesMediaTypeAttribute : Attribute, IActionConstraint
{
    private readonly string _requestHeaderToMatch;
    private readonly MediaTypeCollection _mediaTypes = new();
    private readonly MediaTypeCollection _otherMediaTypes = new();

    public int Order { get; } // TODO: should this be initialized?

    public RequestHeaderMatchesMediaTypeAttribute(string requestHeaderToMatch, string mediaType, params string[] otherMediaTypes)
    {
        _requestHeaderToMatch = requestHeaderToMatch ?? throw new ArgumentNullException(nameof(requestHeaderToMatch));

        // check if the inputted media types are valid media types and add them to the _mediaTypes collection
        AddMediaType(mediaType, _mediaTypes, nameof(mediaType));

        foreach (string otherMediaType in otherMediaTypes)
            AddMediaType(otherMediaType, _otherMediaTypes, nameof(otherMediaTypes));
    }

    public bool Accept(ActionConstraintContext context)
    {
        IHeaderDictionary requestHeaders = context.RouteContext.HttpContext.Request.Headers;

        if (!requestHeaders.ContainsKey(_requestHeaderToMatch))
        {
            HandleUnacceptable(context);
            return false;
        }

        StringValues requestHeader = requestHeaders[_requestHeaderToMatch];
        MediaType parsedRequestMediaType = new MediaType(requestHeader!);

        HandleWildcard(requestHeader, out bool? canDefault);

        if (canDefault.HasValue)
            return canDefault.Value;

        bool canAccept = MediaTypeFound(_mediaTypes, parsedRequestMediaType) || MediaTypeFound(_otherMediaTypes, parsedRequestMediaType);

        if (canAccept)
            return true;

        HandleUnacceptable(context);

        return false;
    }

    private static void HandleUnacceptable(ActionConstraintContext context)
    {
        if (!context.RouteContext.HttpContext.Response.HasStarted)
            context.RouteContext.HttpContext.Response.StatusCode = StatusCodes.Status406NotAcceptable;
    }

    private void AddMediaType(string mediaType, MediaTypeCollection mediaTypeCollection, string argumentName)
    {
        if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? parsedMediaType))
            throw new ArgumentException(argumentName);
            
        mediaTypeCollection.Add(parsedMediaType);
    }

    private void HandleWildcard(StringValues requestHeader, out bool? canDefault)
    {
        canDefault = null;

        // https://www.rfc-editor.org/rfc/rfc9110.html
        bool isWildcard = requestHeader.Count == 1 && requestHeader
            .All(x => !string.IsNullOrWhiteSpace(x) && x.EndsWith(AcceptSubtypeWildcard));

        if (!isWildcard)
            return;

        // TODO: do we want to make this more configurable?
        // do we need to account for other media type defaults?
        canDefault = _mediaTypes.Any(x => x is MediaTypeNames.Application.Json or MediaTypeNames.Application.Xml);
    }

    private bool MediaTypeFound(MediaTypeCollection mediaTypes, MediaType parsedRequestMediaType)
    {
        // if one of the media types is a match, return true
        foreach (string mediaType in mediaTypes)
        {
            MediaType parsedMediaType = new MediaType(mediaType);

            if (parsedRequestMediaType.Equals(parsedMediaType))
                return true;
        }

        return false;
    }
}