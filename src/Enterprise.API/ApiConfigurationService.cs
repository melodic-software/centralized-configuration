using Enterprise.API.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace Enterprise.API;

public static class ApiConfigurationService
{
    public static void Configure(string[] args, Action<ApiConfigurationOptions> configure)
    {
        // This is the entry point to the application.

        ApiConfigurationOptions options = new ApiConfigurationOptions();

        configure.Invoke(options);

        try
        {
            options.EventHandlers.OnPreConfiguration?.Invoke(args);
            WebApplicationBuilder builder = CreateBuilder(args, options);
            

            // Here we can specify URLs to listen on.
            //var urls = new List<string> { "https://localhost:5000" };
            //builder.WebHost.UseUrls(urls.ToArray());
            // This can also be set by using environment variables.

            AddServices(builder, options);
            WebApplication app = BuildApplication(builder, options);

            // WebApplication implements multiple interfaces.
            IHost host = app;
            IApplicationBuilder applicationBuilder = app;
            IEndpointRouteBuilder endpointRouteBuilder = app;
            
            ConfigureRequestPipeline(builder, app, options);
            app.Run();
        }
        catch (Exception ex)
        {
            options.EventHandlers.OnConfigurationError?.Invoke(ex);
            throw;
        }
        finally
        {
            options.EventHandlers.OnPostConfiguration?.Invoke();
        }
    }

    private static WebApplicationBuilder CreateBuilder(string[] args, ApiConfigurationOptions options)
    {
        // This "CreateBuilder" method calls into CreateDefaultBuilder which registers logging providers, and other service defaults.
        // https://github.com/dotnet/aspnetcore/blob/main/src/DefaultBuilder/src/WebHost.cs#L155
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        options.EventHandlers.OnBuilderCreated?.Invoke(builder);
        return builder;
    }

    private static void AddServices(WebApplicationBuilder builder, ApiConfigurationOptions options)
    {
        // Add services to the container.
        // This includes framework services, and application specific services.
        builder.ConfigureServices(options);
        options.EventHandlers.OnServicesConfigured?.Invoke(builder);
    }

    private static WebApplication BuildApplication(WebApplicationBuilder builder, ApiConfigurationOptions options)
    {
        WebApplication app = builder.Build();
        options.EventHandlers.OnWebApplicationBuilt?.Invoke(app);
        return app;
    }

    private static void ConfigureRequestPipeline(WebApplicationBuilder builder, WebApplication app, ApiConfigurationOptions options)
    {
        // Configure the HTTP request (middleware) pipeline.
        app.ConfigureRequestPipeline(builder, options);
        options.EventHandlers.OnRequestPipelineConfigured?.Invoke(app);
    }
}