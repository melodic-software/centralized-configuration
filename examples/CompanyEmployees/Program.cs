using Contracts;
using Enterprise.API;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

ApiConfigurationService.Configure(args, options =>
{
    options.HttpRequestMiddlewareOptions.AddCustomMiddleware = app =>
    {
        
    };

    options.ServiceConfigurationOptions.RegisterCustomServices = (services, builder) =>
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
    };
});