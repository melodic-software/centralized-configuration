using Enterprise.Library.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Enterprise.API.Swagger.Services;

public static class ConflictingActionResolver
{
    /// <summary>
    /// This isn't a great solution.
    /// In most cases, using the [ApiExplorerSettings(IgnoreApi = true)] is a better workaround
    /// along with the use of operation filters.
    /// </summary>
    /// <param name="apiDescriptions"></param>
    /// <returns></returns>
    public static ApiDescription ResolveSimple(IEnumerable<ApiDescription> apiDescriptions) => apiDescriptions.First();

    public static ApiDescription ResolveConflictingActions(IEnumerable<ApiDescription> apiDescriptions)
    {
        // fix fpr possible multiple enumeration (explicit list)
        apiDescriptions = apiDescriptions.ToList();

        // TODO: add logging?

        // for now this is a workaround for controller methods that have route restrictions based on media types
        // [RequestHeaderMatchesMediaType] attribute (action/route constraint)

        ApiDescription firstDescription = apiDescriptions.First();
        List<ApiDescription> otherDescriptions = apiDescriptions.Skip(1).ToList();

        List<ApiResponseType> otherSupportedResponseTypes = otherDescriptions
            .SelectMany(x => x.SupportedResponseTypes.Where(a => a.StatusCode == Status200OK))
            .ToList();

        // this doesn't work because Swashbuckle internally uses a dictionary for status codes
        // so having multiple for each status code will not work...
        firstDescription.SupportedResponseTypes.AddRange(otherSupportedResponseTypes);

        return firstDescription;
    }
}