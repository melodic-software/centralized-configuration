using System.Text;
using Enterprise.API.Constants;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace CompanyEmployees.OutputFormatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaTypeConstants.Csv));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (typeof(CompanyDto).IsAssignableFrom(type) || typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type))
            return base.CanWriteType(type);

        return false;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        HttpResponse response = context.HttpContext.Response;
        StringBuilder buffer = new StringBuilder();

        switch (context.Object)
        {
            case IEnumerable<CompanyDto> dtos:
            {
                foreach (CompanyDto company in dtos)
                    FormatCsv(buffer, company);

                break;
            }
            case CompanyDto companyDto:
                FormatCsv(buffer, companyDto);
                break;
        }

        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, CompanyDto company)
    {
        buffer.AppendLine($"{company.Id},\"{company.Name},\"{company.FullAddress}\"");
    }
}