namespace Configuration.API.ContentNegotiation.Constants;

public static class VendorMediaTypeConstants
{
    public const string HypermediaJson = "application/vnd.company.hateoas+json";
    public const string HypermediaXml = "application/vnd.company.hateoas+xml";
    // NOTE: this ^ wasn't playing nice with output formatters, and would result in a 406
}