namespace Enterprise.API.Caching.Options;

public class CachingConfigurationOptions
{
    public bool EnableRedis { get; set; }
    public string RedisConnectionString { get; set; }
    public string RedisInstanceName { get; set; }

    public CachingConfigurationOptions()
    {
        EnableRedis = true;
        RedisConnectionString = string.Empty;
        RedisInstanceName = string.Empty;
    }
}