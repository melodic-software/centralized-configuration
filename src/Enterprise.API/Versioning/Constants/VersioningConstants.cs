namespace Enterprise.API.Versioning.Constants;

public static class VersioningConstants
{
    public const string CustomVersionRequestHeader = "api-version";
    public const string VersionGroupNameFormat = "'v'VVV"; // alternative: "'v'VV"

    /// <summary>
    /// Versioning via the accept header involves appending a version identifier suffix separated by a semi-colon.
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