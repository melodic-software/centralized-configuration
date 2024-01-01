namespace Enterprise.API.Routing;

public static class RoutePartials
{
    /// <summary>
    /// This can be used to construct a versioned resource identifier.
    /// For example: [Route("api/v{version:apiVersion}/[controller]")]
    /// You may need to define multiple routes if you want to allow for URLs without a version qualifier to fall back to version 1.0.
    /// NOTE: It shouldn't be used by itself.
    /// </summary>
    public const string VersionSegment = "v{version:apiVersion}";
}