using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace Enterprise.API.Controllers.Formatters.Output;

public class OutputFormatterConfigurer : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        HandleNewtonSoftJsonFormatter(options.OutputFormatters);
        AddOutputFormatters(options);
        RemoveOutputFormatters(options);
    }

    public void AddOutputFormatters(MvcOptions options)
    {
        // add any output formatters here
    }

    public void RemoveOutputFormatters(MvcOptions options)
    {
        FormatterCollection<IOutputFormatter> outputFormatters = options.OutputFormatters;

        // by default, string return types are formatted as text/plain (text/html if requested via the Accept header)
        // this behavior can be deleted by removing the StringOutputFormatter
        outputFormatters.RemoveType<StringOutputFormatter>();

        // actions that have a model object return type return 204 No Content when returning null
        // this behavior can be deleted by removing the HttpNoContentOutputFormatter

        // NOTE: without the HttpNoContentOutputFormatter, null objects are formatted using the configured formatter
        // the JSON formatter returns a response with a body of null
        // the XML formatter returns an empty XML element with the attribute xsi:nil="true" set

        //outputFormatters.RemoveType<HttpNoContentOutputFormatter>();
    }

    private void HandleNewtonSoftJsonFormatter(FormatterCollection<IOutputFormatter> outputFormatters)
    {
        NewtonsoftJsonOutputFormatter? newtonSoftJsonOutputFormatter = outputFormatters
            .OfType<NewtonsoftJsonOutputFormatter>()
            .FirstOrDefault();

        if (newtonSoftJsonOutputFormatter == null)
            return;

        // remove text/json as it isn't the approved media type for working with JSON at an API level
        newtonSoftJsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
    }
}