namespace Enterprise.API.Versioning.Constants;

public static class VersioningConstants
{
    public const string CustomVersionRequestHeader = "api-version";

    /// <summary>
    /// The default name used by the reader is "api-version".
    /// </summary>
    public const string VersionQueryStringParameterName = "api-version";

    /// <summary>
    /// This is the .NET default.
    /// Example: "application/json;v=2.0".
    /// </summary>
    public const string MediaTypeVersionParameterName = "v";

    /// <summary>
    /// This is the versioning format for the group name.
    /// This will generate a three digit scheme prefixed by a "v".
    /// Versions v000 -> v999 are supported.
    /// This number represents the "MAJOR" version incremented when there is a breaking change.
    /// </summary>
    public const string VersionGroupNameFormat = "'v'VVV";

    /// <summary>
    /// Versioning via the accept header involves appending a version identifier suffix separated by a semicolon.
    /// For example: "application/json;version=v1";
    /// </summary>
    /// <param name="acceptHeaderValue"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public static string VersionedAcceptHeader(string acceptHeaderValue, string version) => $"{acceptHeaderValue};version={version}";

    /// <summary>
    /// Create a versioned vendor media type.
    /// For example: "application/vnd.acme.book.v1+json"
    /// </summary>
    /// <param name="companyName"></param>
    /// <param name="subType"></param>
    /// <param name="version"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static string VersionedVendorMediaType(string companyName, string subType, string version, string suffix) => 
        $"application/vnd.{companyName}.{subType}.{version}+{suffix}";
}