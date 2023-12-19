using System.Text.Json;
using System.Text.Json.Serialization;
using Enterprise.API.Controllers.Formatters.Input;
using Enterprise.API.Controllers.Formatters.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Enterprise.API.Controllers.Formatters;

public static class FormatterConfigurationService
{
    public static IMvcBuilder ConfigureFormatters(this IMvcBuilder builder)
    {
        ConfigureInputFormatters(builder);
        ConfigureOutputFormatters(builder);
        Configure(builder);

        return builder;
    }

    private static IMvcBuilder ConfigureInputFormatters(this IMvcBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, InputFormatterConfigurer>());
            
        return builder;
    }

    private static IMvcBuilder ConfigureOutputFormatters(this IMvcBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, OutputFormatterConfigurer>());

        return builder;
    }

    private static IMvcBuilder Configure(this IMvcBuilder builder)
    {
        //builder.AddXmlSerializerFormatters();

        // add XML input and output formatters
        // the data contract serializer supports types like DateTimeOffset
        // the regular XML serializer requires that a type is designed in a specific way in order to completely serialize
        // it requires a default public constructor, public read/write members, etc.
        builder.AddXmlDataContractSerializerFormatters();

        // configure System.Text.Json-based formatters
        builder.AddJsonOptions(options =>
        {
            JsonSerializerOptions serializerOptions = options.JsonSerializerOptions;

            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.PropertyNameCaseInsensitive = true;
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // prevents cyclical data references
            serializerOptions.WriteIndented = true;
        });

        return builder;
    }
}