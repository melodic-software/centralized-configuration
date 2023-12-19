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
            // here we are configuring how the [ApiController] attribute should behave

            // NOTE: this suppresses the default model state validation that is implemented due to the existence of the ApiController attribute
            //options.SuppressModelStateInvalidFilter = true;

            // this is what will execute when the model state is invalid
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

        // create a validation problem details object
        ProblemDetailsFactory problemDetailsFactory = requestServices.GetRequiredService<ProblemDetailsFactory>();

        // this will translate the model data to the RFC format
        ValidationProblemDetails validationProblemDetails = problemDetailsFactory
            .CreateValidationProblemDetails(httpContext, context.ModelState);

        // add additional info not added by default
        validationProblemDetails.Detail = ProblemDetailsDetail;
        validationProblemDetails.Instance = requestPath;

        // report invalid model state responses as validation issues
        //validationProblemDetails.Type = ProblemDetailsType;
        validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity; // use 422 instead of the default 400
        validationProblemDetails.Title = ProblemDetailsTitle;

        IActionResult result = new UnprocessableEntityObjectResult(validationProblemDetails)
        {
            ContentTypes = { ContentTypeConstants.ApplicationProblemPlusJson } // part of the RFC
        };

        return result;
    }

    private static void CustomizeClientErrorMapping(ApiBehaviorOptions options)
    {
        // we can customize the message and URL for specific status codes in error responses
        options.ClientErrorMapping[StatusCodes.Status500InternalServerError] = new ClientErrorData
        {
            Link = InternalServerErrorLink,
            Title = InternalServerErrorMessage
        };

        // customize just the link (or title)
        options.ClientErrorMapping[StatusCodes.Status401Unauthorized].Link = UnauthorizedLink;
    }
}