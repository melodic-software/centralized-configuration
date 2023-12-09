using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Enterprise.API.Problems;
// NOTE: this allows for complete control over problem detail object construction.

public class CustomProblemDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions? _options;

    public CustomProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
    {
        _options = options?.Value;
    }

    public override ProblemDetails CreateProblemDetails(HttpContext context, int? statusCode = null, string? title = null,
        string? type = null, string? detail = null, string? instance = null)
    {
        // this is the standard (non customized) problem details
        ProblemDetails problemDetails = new ProblemDetails
        {
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance,
            Status = statusCode
        };

        if (_options != null)
        {
            // use/overwrite the default values with those configured (if applicable)
            if (statusCode.HasValue && _options.ClientErrorMapping.TryGetValue(statusCode.Value, out ClientErrorData? clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }
        }

        string? traceId = context?.TraceIdentifier;

        if (traceId != null)
            problemDetails.Extensions.Add("traceId", traceId);

        // customizations can be added via extensions
        //problemDetails.Extensions.Add("customValue", "value");
        //problemDetails.Extensions.Add("customValue2", new { TestProperty = "test" });

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext context, ModelStateDictionary modelStateDictionary,
        int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
    {
        // this is the standard (non customized) validation problem details
        ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Title = title,
            Detail = detail,
            Status = statusCode ?? StatusCodes.Status400BadRequest,
            Instance = instance,
            Type = type
        };

        return validationProblemDetails;
    }
}