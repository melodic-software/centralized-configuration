using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Enterprise.API.Cors.Options;

public class CorsConfigurationOptions
{
    public bool EnableCors { get; set; }
    public List<string> AllowedOrigins { get; set; }
    public Action<CorsOptions>? ConfigureCustom { get; set; }

    public CorsConfigurationOptions()
    {
        EnableCors = true;
        AllowedOrigins = new List<string>();
        ConfigureCustom = null;
    }
}