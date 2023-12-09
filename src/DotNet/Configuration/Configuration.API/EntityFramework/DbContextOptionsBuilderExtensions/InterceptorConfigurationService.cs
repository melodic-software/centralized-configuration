using Configuration.API.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Configuration.API.EntityFramework.DbContextOptionsBuilderExtensions;

public static class InterceptorConfigurationService
{
    public static void ConfigureInterceptors(this DbContextOptionsBuilder optionsBuilder, IServiceCollection serviceCollection)
    {
        List<IInterceptor> interceptors = new List<IInterceptor>
        {
            new CustomDbCommandInterceptor()
        };

        optionsBuilder.AddInterceptors(interceptors);
    }
}