using Microsoft.AspNetCore.Mvc.Formatters;

namespace Enterprise.API.Controllers.Options
{
    public class FormatterConfigurationOptions
    {
        public List<IInputFormatter> InputFormatters { get; set; } = new();
        public List<IOutputFormatter> OutputFormatters { get; set; } = new();
    }
}
