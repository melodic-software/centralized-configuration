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
            // TODO: Disable this by default, but make it configurable.
            options.SuppressModelStateInvalidFilter = false;

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
            ContentTypes = { ContentTypeConstants.ApplicationProblemPlusJson } // Part of the RFC.
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