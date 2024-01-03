using static Enterprise.Constants.CompanyConstants;

namespace Configuration.API.ContentNegotiation.Constants;

public static class VendorMediaTypeConstants
{
    public const string HypermediaJson = $"application/vnd.{VendorMediaTypeCompanyName}.hateoas+json";

    /// <summary>
    /// This cannot be used with dynamic or anonymous types
    /// unless a custom XML output formatter has been added to handle dynamic serialization.
    /// </summary>
    public const string HypermediaXml = $"application/vnd.{VendorMediaTypeCompanyName}.hateoas+xml";
}