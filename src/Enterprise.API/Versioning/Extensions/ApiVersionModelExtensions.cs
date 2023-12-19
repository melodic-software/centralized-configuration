using Asp.Versioning;

namespace Enterprise.API.Versioning.Extensions;

public static class ApiVersionModelExtensions
{
    public static bool NoVersionAvailable(this ApiVersionModel apiVersionModel)
    {
        bool noVersioningAvailable = !apiVersionModel.DeclaredApiVersions.Any() &&
                                     !apiVersionModel.ImplementedApiVersions.Any() &&
                                     !apiVersionModel.DeprecatedApiVersions.Any() &&
                                     !apiVersionModel.SupportedApiVersions.Any();

        return noVersioningAvailable;
    }
}