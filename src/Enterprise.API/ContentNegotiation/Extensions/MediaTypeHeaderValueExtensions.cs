using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using static System.StringComparison;

namespace Enterprise.API.ContentNegotiation.Extensions;

public static class MediaTypeHeaderValueExtensions
{
    private const string HATEOAS = "hateoas";

    public static bool EndsWithHATEOAS(this MediaTypeHeaderValue mediaType)
    {
        StringSegment subTypeWithoutSuffix = mediaType.SubTypeWithoutSuffix;
        bool endsWithHateos = subTypeWithoutSuffix.EndsWith(HATEOAS, InvariantCultureIgnoreCase);
        return endsWithHateos;
    }
    
    public static StringSegment GetPrimaryMediaType(this MediaTypeHeaderValue mediaType, bool endsWithHATEOAS)
    {
        StringSegment subTypeWithoutSuffix = mediaType.SubTypeWithoutSuffix;

        StringSegment primaryMediaType = endsWithHATEOAS ?
            subTypeWithoutSuffix.Substring(0, subTypeWithoutSuffix.Length - HATEOAS.Length) :
            subTypeWithoutSuffix;

        return primaryMediaType;
    }
}