using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enterprise.API.Controllers.Formatters.Input;

public class InputFormatterConfigurer : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        // this adds support for JSON patch using Newtonsoft.Json while leaving other input and output formatters unchanged
        // in all other cases we want to be using the System.Text.Json library by Microsoft
        // https://learn.microsoft.com/en-us/aspnet/core/web-api/jsonpatch
        // https://datatracker.ietf.org/doc/html/rfc6902
        options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
    }

    private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    {
        ServiceProvider builder = new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson()
            .Services.BuildServiceProvider();

        NewtonsoftJsonPatchInputFormatter formatter = builder
            .GetRequiredService<IOptions<MvcOptions>>()
            .Value
            .InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();

        return formatter;
    }
}