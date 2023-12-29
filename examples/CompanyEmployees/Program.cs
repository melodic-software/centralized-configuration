using Enterprise.API;

ApiConfigurationService.Configure(args, options =>
{
    options.HttpRequestMiddlewareOptions.AddCustomMiddleware = app =>
    {
        
    };
});