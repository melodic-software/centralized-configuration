using Enterprise.API.Controllers.Formatters.Input;
using Enterprise.API.Controllers.Formatters.Output;
using Enterprise.API.Controllers.Options;
using Enterprise.Serialization.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Enterprise.API.Controllers.Formatters;

public static class FormatterConfigurationService
{
    public static IMvcBuilder ConfigureFormatters(this IMvcBuilder builder, FormatterConfigurationOptions formatterConfigOptions)
    {
        AddXmlInputOutputFormatters(builder);

        ConfigureInputFormatters(builder, formatterConfigOptions.InputFormatters);
        ConfigureOutputFormatters(builder, formatterConfigOptions.OutputFormatters);
        Configure(builder);

        return builder;
    }

    private static void AddXmlInputOutputFormatters(IMvcBuilder builder)
    {
        builder.AddXmlSerializerFormatters();

        // Add XML input and output formatters.
        // The data contract serializer supports types like DateTimeOffset.
        // The regular XML serializer requires that a type is designed in a specific way in order to completely serialize.
        // It requires a default public constructor, public read/write members, etc.
        builder.AddXmlDataContractSerializerFormatters();
    }

    private static void ConfigureInputFormatters(IMvcBuilder builder, List<IInputFormatter> inputFormatters)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<MvcOptions>, InputFormatterConfigurer>(
                provider => new InputFormatterConfigurer(inputFormatters))
        );
    }

    private static void ConfigureOutputFormatters(IMvcBuilder builder, List<IOutputFormatter> outputFormatters)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<MvcOptions>, OutputFormatterConfigurer>(
                provider => new OutputFormatterConfigurer(outputFormatters)
            ));
    }

    private static void Configure(IMvcBuilder builder)
    {
        // configure System.Text.Json-based formatters
        builder.AddJsonOptions(options =>
        {
            JsonSerializerOptions serializerOptions = options.JsonSerializerOptions;

            JsonSerializerOptions defaultSerializerOptions = JsonSerializerOptionsService.GetDefaultOptions();

            serializerOptions.PropertyNamingPolicy = defaultSerializerOptions.PropertyNamingPolicy;
            serializerOptions.DictionaryKeyPolicy = defaultSerializerOptions.DictionaryKeyPolicy;
            serializerOptions.PropertyNameCaseInsensitive = defaultSerializerOptions.PropertyNameCaseInsensitive;
            serializerOptions.ReferenceHandler = defaultSerializerOptions.ReferenceHandler;
            serializerOptions.WriteIndented = defaultSerializerOptions.WriteIndented;
        });
    }
}