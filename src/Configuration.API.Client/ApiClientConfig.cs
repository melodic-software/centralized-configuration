using System.Reflection;

namespace Configuration.API.Client;

public static class ApiClientConfig
{
    public static Func<Assembly> GetAssembly = () =>
    {
        // this assembly lives in the project dedicated to API clients (http), and model contracts
        Assembly apiClientAssembly = typeof(ApiClientConfig).Assembly;

        return apiClientAssembly;
    };
}