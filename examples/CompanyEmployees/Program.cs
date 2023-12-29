using Enterprise.API;

ApiConfigurationService.Configure(args, options =>
{
    options.HttpRequestMiddlewareOptions.AddCustomMiddleware = app =>
    {
        app.Run(async context =>
        {
            await context.Response.WriteAsync("Hello from middleware component.");
        });
    };
});