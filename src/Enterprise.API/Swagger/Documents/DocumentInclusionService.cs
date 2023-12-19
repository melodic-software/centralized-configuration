using Asp.Versioning;
using Enterprise.API.Versioning.Extensions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Enterprise.API.Swagger.Documents;

public static class DocumentInclusionService
{
    public static bool CanIncludeDocument(string documentName, ApiDescription apiDescription)
    {
        // extension method from the versioned API explorer package
        // we want both explicit (via API version attribute) and implicit (default version) defined versions

        // there was a warning about a bitwise operation on an enum which is not marked with the [Flags] attribute
        // (ApiVersionMapping)3 is synonymous with ApiVersionMapping.Explicit | ApiVersionMapping.Implicit;
        ApiVersionMapping apiVersionMapping = (ApiVersionMapping)3;

        ApiVersionMetadata apiVersionMetadata = apiDescription.ActionDescriptor.GetApiVersionMetadata();
        ApiVersionModel actionApiVersionModel = apiVersionMetadata.Map(apiVersionMapping);

        // should be present in every version
        if (actionApiVersionModel.IsApiVersionNeutral)
            return true;

        if (actionApiVersionModel.NoVersionAvailable())
            return true;

        List<ApiVersion> matchingVersions;

        if (actionApiVersionModel.DeclaredApiVersions.Any())
        {
            matchingVersions = actionApiVersionModel.DeclaredApiVersions
                .Where(apiVersion => DocumentNameMatches(apiVersion, documentName))
                .ToList();

            return matchingVersions.Any();
        }

        matchingVersions = actionApiVersionModel.ImplementedApiVersions
            .Where(apiVersion => DocumentNameMatches(apiVersion, documentName))
            .ToList();

        return matchingVersions.Any();
    }

    private static bool DocumentNameMatches(ApiVersion apiVersion, string documentName)
    {
        bool matches = $"v{apiVersion.MajorVersion}" == documentName;

        return matches;
    }
}