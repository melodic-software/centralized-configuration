using System.Reflection;
using Enterprise.Logging.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;

namespace Enterprise.Logging.Serilog;

public static class SerilogConfigurationService
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder, bool clearExistingProviders = false)
    {
        if (clearExistingProviders)
            builder.Logging.ClearProviders();

        Assembly? assembly = Assembly.GetEntryAssembly();

        if (assembly == null)
            throw new Exception("Invalid assembly reference!");

        string? assemblyName = assembly.GetName().Name;

        if (string.IsNullOrWhiteSpace(assemblyName))
            throw new Exception("The assembly name is invalid!");

        builder.Logging.RemoveConsoleLogger();

        builder.Host.UseSerilog((context, loggerConfig) =>
                loggerConfig
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.WithProperty("Application", assemblyName)
                    .Enrich.WithExceptionDetails()
                    .Enrich.FromLogContext()
                    .Enrich.With<ActivityEnricher>()
                    .WriteTo.Console()
                    .WriteTo.Debug()
                    .WriteTo.Seq("http://localhost:5341") // local docker container
        );
    }
}