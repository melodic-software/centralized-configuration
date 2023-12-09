using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.API.Controllers.Extensions;

public static class ApiControllerExtensions
{
    public static IActionResult BadDataShapingRequest(this ControllerBase controller, ProblemDetailsFactory problemDetailsFactory, string? properties)
    {
        ProblemDetails problemDetails = problemDetailsFactory.CreateProblemDetails(
            controller.HttpContext,
            statusCode: StatusCodes.Status400BadRequest,
            detail: $"Not all requested data shaping fields exist on the resource: {properties}"
        );

        return controller.BadRequest(problemDetails);
    }
}