using System.Text.Json;
using System.Text.Json.Serialization;

namespace Enterprise.Serialization.Json;

public static class JsonSerializerOptionsService
{
    public static JsonSerializerOptions GetDefaultOptions()
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles, // This prevents cyclical data references.
            WriteIndented = true
        };

        return options;
    }
}