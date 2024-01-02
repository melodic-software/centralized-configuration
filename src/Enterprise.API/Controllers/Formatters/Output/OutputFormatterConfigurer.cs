using Enterprise.API.Controllers.Formatters.Output.Custom;
using Enterprise.API.Versioning.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace Enterprise.API.Controllers.Formatters.Output;

public class OutputFormatterConfigurer(List<IOutputFormatter> outputFormatters) : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        List<IOutputFormatter> initialFormatters = options.OutputFormatters.ToList();

        HandleNewtonSoftJsonFormatter(options);
        RemoveOutputFormatters(options);
        AddOutputFormatters(options);
        AddVersionMediaTypeDelegatingFormatter(options);
        //ReOrderOutputFormatters(options);

        List<IOutputFormatter> configuredFormatters = options.OutputFormatters.ToList();
    }

    private void HandleNewtonSoftJsonFormatter(MvcOptions options)
    {
        NewtonsoftJsonOutputFormatter? newtonSoftJsonOutputFormatter = options.OutputFormatters
            .OfType<NewtonsoftJsonOutputFormatter>()
            .FirstOrDefault();

        if (newtonSoftJsonOutputFormatter == null)
            return;

        // Remove text/json as it isn't the approved media type for working with JSON at an API level.
        newtonSoftJsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
    }

    public void RemoveOutputFormatters(MvcOptions options)
    {
        // By default, string return types are formatted as text/plain (text/html if requested via the Accept header).
        // This behavior can be deleted by removing the StringOutputFormatter.
        options.OutputFormatters.RemoveType<StringOutputFormatter>();

        // Actions that have a model object return type return 204 No Content when returning null.
        // This behavior can be deleted by removing the HttpNoContentOutputFormatter.

        // NOTE: Without the HttpNoContentOutputFormatter, null objects are formatted using the configured formatter.
        // The JSON formatter returns a response with a body of null.
        // The XML formatter returns an empty XML element with the attribute xsi:nil="true" set.

        //options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
    }

    public void AddOutputFormatters(MvcOptions options)
    {
        // Adds output formatters here.

        // Add custom (application specific) output formatters.
        foreach (IOutputFormatter outputFormatter in outputFormatters)
            options.OutputFormatters.Add(outputFormatter);
    }

    private void AddVersionMediaTypeDelegatingFormatter(MvcOptions options)
    {
        List<IOutputFormatter> currentFormatters = options.OutputFormatters.ToList();

        // This parses and strips out the version parameter.
        // Then delegates to any and all existing formatters.
        // It is important that this is added last.

        options.OutputFormatters.Add(new MediaTypeVersionDelegatingFormatter(currentFormatters, VersioningConstants.MediaTypeVersionParameterName));
    }

    private void ReOrderOutputFormatters(MvcOptions options)
    {
        // NOTE: The System.Text.Json formatter should already be the default.
        // But this will ensure it is the first output formatter in the list.

        // Find the System.Text.Json formatter.
        SystemTextJsonOutputFormatter? jsonFormatter = options.OutputFormatters
            .OfType<SystemTextJsonOutputFormatter>()
            .FirstOrDefault();

        if (jsonFormatter == null)
            return;

        // Remove the existing JSON formatter.
        options.OutputFormatters.Remove(jsonFormatter);

        // Re-insert it at the beginning of the collection.
        options.OutputFormatters.Insert(0, jsonFormatter);
    }
}