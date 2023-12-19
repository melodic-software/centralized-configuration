using IdentityModel.Client;

namespace Enterprise.API.Security.OAuth;

public class DiscoveryDocumentService
{
    public static async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(string authority)
    {
        DiscoveryDocumentResponse discoveryDocumentResponse;

        try
        {
            DiscoveryCache discoveryCache = new DiscoveryCache(authority);
            discoveryDocumentResponse = await discoveryCache.GetAsync();

            // we probably don't need the cache here, but leaving it as an example
            // if the discovery document were to be accessed frequently, the cache is a better choice
            //HttpClient client = new HttpClient(); // TODO: should this come from an HTTP client factory?
            //discoveryDocumentResponse = await client.GetDiscoveryDocumentAsync(authority);
        }
        catch (Exception)
        {
            // what happens if we can't reach the authority?
            // TODO: add retry logic, handle connection errors as best as possible
            throw;
        }

        return discoveryDocumentResponse;
    }

    public static void ValidateDiscoveryDocumentResponse(DiscoveryDocumentResponse discoveryDocResponse)
    {
        string? authorizeEndpoint = discoveryDocResponse.AuthorizeEndpoint;
        string? tokenEndpoint = discoveryDocResponse.TokenEndpoint;

        bool discoveryDocResponseIsInvalid = string.IsNullOrWhiteSpace(authorizeEndpoint) ||
                                             string.IsNullOrWhiteSpace(tokenEndpoint);

        if (discoveryDocResponseIsInvalid)
            throw new Exception("Discovery document response is invalid!");
    }
}