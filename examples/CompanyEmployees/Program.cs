using Contracts;
using Enterprise.API;
using LoggerService;

ApiConfigurationService.Configure(args, options =>
{
    options.HttpRequestMiddlewareOptions.AddCustomMiddleware = app =>
    {
        
    };

    options.ServiceConfigurationOptions.RegisterCustomServices = (services, builder) =>
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    };
});