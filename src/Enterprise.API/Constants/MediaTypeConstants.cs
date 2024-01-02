using System.Net.Mime;

namespace Enterprise.API.Constants;

public static class MediaTypeConstants
{
    public const string Json = MediaTypeNames.Application.Json; // "application/json"
    public const string JsonPatch = "application/json-patch+json";
    public const string Xml = MediaTypeNames.Application.Xml; // "application/xml"
    public const string Csv = MediaTypeNames.Text.Csv; // "text/csv"
}