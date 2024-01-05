using Enterprise.API.Constants;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using static Enterprise.API.Constants.StatusCodeLinkConstants;
using static Enterprise.API.Controllers.Behavior.ApiBehaviorConstants;

namespace Enterprise.API.Controllers.Behavior;

public static class ApiBehaviorConfigurationService
{
    public static void ConfigureApiBehavior(this IMvcBuilder builder)
    {
        builder.ConfigureApiBehaviorOptions(options =>
        {
            // Here we are configuring how the [ApiController] attribute should behave.

            // NOTE: This suppresses the default model state validation that is implemented due to the existence of the ApiController attribute.
            // This requires controllers to check for null model/DTO objects.
            // If nullable context is enabled in the .csproj, specifying a nullable input model will allow null request bodies and not trigger this validation.
            // https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references#nullable-contexts
            // When set to true, this allows us to manually check and return a 422 instead of the default 400 (which is less accurate).
            // When set to false, model validations are automatically handled and returned as ValidationProblem().
            // TODO: Enable this by default, but make it configurable.
            options.SuppressModelStateInvalidFilter = true;

            // This is what will execute when the model state is invalid.
            options.InvalidModelStateResponseFactory = GetInvalidModelStateResponseFactory;

            //CustomizeClientErrorMapping(options);
        });
    }

    private static IActionResult GetInvalidModelStateResponseFactory(ActionContext context)
    {
        HttpContext httpContext = context.HttpContext;
        PathString requestPath = httpContext.Request.Path;
        IServiceProvider requestServices = httpContext.RequestServices;

        // https://datatracker.ietf.org/doc/html/rfc7807

        // Create a validation problem details object.
        ProblemDetailsFactory problemDetailsFactory = requestServices.GetRequiredService<ProblemDetailsFactory>();

        // This will translate the model data to the RFC format.
        ValidationProblemDetails validationProblemDetails = problemDetailsFactory
            .CreateValidationProblemDetails(httpContext, context.ModelState);

        // Add additional info not added by default.
        validationProblemDetails.Detail = ProblemDetailsDetail;
        validationProblemDetails.Instance = requestPath;

        // Report invalid model state responses as validation issues.
        //validationProblemDetails.Type = ProblemDetailsType;
        validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity; // Use 422 instead of the default 400.
        validationProblemDetails.Title = ProblemDetailsTitle;

        IActionResult result = new UnprocessableEntityObjectResult(validationProblemDetails)
        {
            ContentTypes =
            {
                // Part of the RFC.
                ContentTypeConstants.ApplicationProblemPlusJson, 
                ContentTypeConstants.ApplicationProblemPlusJson
            }
        };

        return result;
    }

    private static void CustomizeClientErrorMapping(ApiBehaviorOptions options)
    {
        // We can customize the message and URL for specific status codes in error responses.
        options.ClientErrorMapping[StatusCodes.Status500InternalServerError] = new ClientErrorData
        {
            Link = InternalServerErrorLink,
            Title = InternalServerErrorMessage
        };

        // Customize just the link (or title).
        options.ClientErrorMapping[StatusCodes.Status401Unauthorized].Link = UnauthorizedLink;
    }
}