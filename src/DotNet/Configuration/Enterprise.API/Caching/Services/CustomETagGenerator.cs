using System.Text;
using Enterprise.Library.Services;
using Marvin.Cache.Headers;
using Marvin.Cache.Headers.Extensions;
using Marvin.Cache.Headers.Interfaces;

namespace Enterprise.API.Caching.Services;

public class CustomETagGenerator : IETagGenerator
{
    private readonly DefaultStrongETagGenerator _defaultStrongETagGenerator;

    public CustomETagGenerator(IStoreKeySerializer storeKeySerializer)
    {
        _defaultStrongETagGenerator = new DefaultStrongETagGenerator(storeKeySerializer);
    }

    public async Task<ETag> GenerateETag(StoreKey storeKey, string responseBodyContent)
    {
        ETag eTag = await GenerateDefaultETag(storeKey, responseBodyContent);
        //ETag customETag = await GenerateCustomETag(storeKey, responseBodyContent);
        return eTag;
    }

    private async Task<ETag> GenerateDefaultETag(StoreKey storeKey, string responseBodyContent)
    {
        ETag? eTag = await _defaultStrongETagGenerator.GenerateETag(storeKey, responseBodyContent);

        return eTag;
    }

    private Task<ETag> GenerateCustomETag(StoreKey storeKey, string responseBodyContent)
    {
        string? storeKeyString = storeKey.ToString();

        ETagType eTagType = ETagType.Strong;
        byte[] storeKeyBytes = Encoding.UTF8.GetBytes(storeKeyString);
        byte[] responseBodyBytes = Encoding.UTF8.GetBytes(responseBodyContent);
        byte[] combinedBytes = ByteArrayService.Combine(storeKeyBytes, responseBodyBytes);
        string? md5Hash = combinedBytes.GenerateMD5Hash();

        ETag eTag = new ETag(eTagType, md5Hash);

        return Task.FromResult(eTag);
    }
}