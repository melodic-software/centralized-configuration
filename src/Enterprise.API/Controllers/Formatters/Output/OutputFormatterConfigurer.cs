using Enterprise.API.Controllers.Formatters.Output.Custom;
using Enterprise.API.Versioning.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace Enterprise.API.Controllers.Formatters.Output;

public class OutputFormatterConfigurer : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        List<IOutputFormatter> initialFormatters = options.OutputFormatters.ToList();

        HandleNewtonSoftJsonFormatter(options.OutputFormatters);
        RemoveOutputFormatters(options.OutputFormatters);
        AddOutputFormatters(options.OutputFormatters);
        AddVersionMediaTypeDelegatingFormatter(options.OutputFormatters);
        //ReOrderOutputFormatters(options.OutputFormatters);

        List<IOutputFormatter> configuredFormatters = options.OutputFormatters.ToList();
    }

    private void HandleNewtonSoftJsonFormatter(FormatterCollection<IOutputFormatter> outputFormatters)
    {
        NewtonsoftJsonOutputFormatter? newtonSoftJsonOutputFormatter = outputFormatters
            .OfType<NewtonsoftJsonOutputFormatter>()
            .FirstOrDefault();

        if (newtonSoftJsonOutputFormatter == null)
            return;

        // Remove text/json as it isn't the approved media type for working with JSON at an API level.
        newtonSoftJsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
    }

    public void RemoveOutputFormatters(FormatterCollection<IOutputFormatter> outputFormatters)
    {
        // By default, string return types are formatted as text/plain (text/html if requested via the Accept header).
        // This behavior can be deleted by removing the StringOutputFormatter.
        outputFormatters.RemoveType<StringOutputFormatter>();

        // Actions that have a model object return type return 204 No Content when returning null.
        // This behavior can be deleted by removing the HttpNoContentOutputFormatter.

        // NOTE: Without the HttpNoContentOutputFormatter, null objects are formatted using the configured formatter.
        // The JSON formatter returns a response with a body of null.
        // The XML formatter returns an empty XML element with the attribute xsi:nil="true" set.

        //outputFormatters.RemoveType<HttpNoContentOutputFormatter>();
    }

    public void AddOutputFormatters(FormatterCollection<IOutputFormatter> outputFormatters)
    {
        // Add output formatters here.
    }

    private void AddVersionMediaTypeDelegatingFormatter(FormatterCollection<IOutputFormatter> outputFormatters)
    {
        List<IOutputFormatter> currentFormatters = outputFormatters.ToList();

        // This parses and strips out the version parameter.
        // Then delegates to any and all existing formatters.
        // It is important that this is added last.

        outputFormatters.Add(new MediaTypeVersionDelegatingFormatter(currentFormatters, VersioningConstants.MediaTypeVersionParameterName));
    }

    private void ReOrderOutputFormatters(FormatterCollection<IOutputFormatter> outputFormatters)
    {
        // NOTE: The System.Text.Json formatter should already be the default.
        // But this will ensure it is the first output formatter in the list.

        // Find the System.Text.Json formatter.
        SystemTextJsonOutputFormatter? jsonFormatter = outputFormatters
            .OfType<SystemTextJsonOutputFormatter>()
            .FirstOrDefault();

        if (jsonFormatter == null)
            return;
        
        // Remove the existing JSON formatter.
        outputFormatters.Remove(jsonFormatter);

        // Re-insert it at the beginning of the collection.
        outputFormatters.Insert(0, jsonFormatter);
    }
}