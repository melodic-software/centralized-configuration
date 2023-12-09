using Enterprise.API.Constants;
using static Enterprise.Constants.CharacterConstants;

namespace Enterprise.API.ContentNegotiation;

public static class VendorMediaTypeService
{
    public static string GetMediaType(string companyName, string partial)
    {
        string vendorPrefix = GetVendorPrefix(companyName);
        string mediaType = $"{companyName}{Period}{partial}";
        return mediaType;
    }

    public static string GetMediaType(string companyName, List<string> subTypes, string? suffix)
    {
        string concatenatedSubTypes = string.Join(Period, subTypes);
        string mediaType = GetMediaType(companyName, concatenatedSubTypes, suffix);
        return mediaType;
    }

    public static string GetMediaType(string companyName, string subTypes, string? suffix)
    {
        // https://en.wikipedia.org/wiki/Media_type

        string vendorPrefix = GetVendorPrefix(companyName);

        if (subTypes.StartsWith(Period))
            subTypes = subTypes.Substring(1);

        string mediaType = $"{vendorPrefix}{Period}{subTypes}";

        if (string.IsNullOrEmpty(suffix))
            return mediaType;

        if (!suffix.StartsWith(PlusSign))
        {
            mediaType += $"{PlusSign}{suffix}";
        }
        else
        {
            mediaType += suffix;
        }

        return mediaType;
    }

    public static string GetVendorPrefix(string companyName)
    {
        string type = MediaTypePrefixTypeConstants.Application;
        string treeSubtype = MediaTypeTreeSubtypeConstants.Vendor;
        string vendorPrefix = $"{type}.{treeSubtype}.{companyName}";
        return vendorPrefix;
    }
}