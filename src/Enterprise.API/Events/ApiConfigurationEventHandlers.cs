using Microsoft.AspNetCore.Builder;

namespace Enterprise.API.Events;

public class ApiConfigurationEventHandlers
{
    public Func<string[], Task>? OnPreConfiguration { get; set; }
    public Func<WebApplicationBuilder, Task>? OnBuilderCreated { get; set; }
    public Func<WebApplicationBuilder, Task>? OnServicesConfigured { get; set; }
    public Func<WebApplication, Task>? OnWebApplicationBuilt { get; set; }
    public Func<WebApplication, Task>? OnRequestPipelineConfigured { get; set; }
    public Func<Exception, Task>? OnConfigurationError { get; set; }
    public Func<Task>? OnPostConfiguration { get; set; }

    public ApiConfigurationEventHandlers()
    {
        OnPreConfiguration = args =>
        {
            Console.WriteLine("PRE-CONFIGURATION");
            return Task.CompletedTask;
        };

        OnBuilderCreated = builder =>
        {
            Console.WriteLine("BUILDER CREATED");
            return Task.CompletedTask;
        };

        OnServicesConfigured = builder =>
        {
            Console.WriteLine("SERVICES CONFIGURED");
            return Task.CompletedTask;
        };

        OnWebApplicationBuilt = app =>
        {
            Console.WriteLine("WEB APPLICATION BUILT");
            return Task.CompletedTask;
        };

        OnRequestPipelineConfigured = app =>
        {
            Console.WriteLine("REQUEST PIPELINE CONFIGURED");
            return Task.CompletedTask;
        };

        OnConfigurationError = exception =>
        {
            Console.WriteLine($"EXCEPTION: {exception.Message}");
            return Task.CompletedTask;
        };

        OnPostConfiguration = () =>
        {
            Console.WriteLine("CONFIGURATION COMPLETE");
            return Task.CompletedTask;
        };
    }
}