using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Enterprise.Traceability;

public static class OpenTelemetryConfigurationService
{
    public static void ConfigureOpenTelemetry(this WebApplicationBuilder builder)
    {
        // https://opentelemetry.io/docs/instrumentation/net/

        // TODO: here's some courses that explore this further
        // https://app.pluralsight.com/library/courses/dot-net-6-application-performance-measuring-monitoring/table-of-contents
        // https://app.pluralsight.com/library/courses/opentelemetry-grafana-observability/table-of-contents
        // https://app.pluralsight.com/library/courses/dot-net-diagnostics-applications-best-practices/table-of-contents

        string applicationName = builder.Environment.ApplicationName;
        string jaegerUrl = "http://localhost:4317";

        builder.Services.ConfigureOpenTelemetryTracerProvider(b =>
        {
            ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault().AddService(applicationName);
            TracerProviderBuilder tracerProviderBuilder = b.SetResourceBuilder(resourceBuilder);

            tracerProviderBuilder.AddAspNetCoreInstrumentation();
            tracerProviderBuilder.AddEntityFrameworkCoreInstrumentation();

            // TODO: http client instrumentation can be added if the application makes HTTP calls

            tracerProviderBuilder.AddOtlpExporter(opts =>
            {
                opts.Endpoint = new Uri(jaegerUrl);
            });
        });
    }
}