using Enterprise.API.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Enterprise.API.ErrorHandling.Controllers;
// NOTE: this has to be routed to in the request pipeline registration with "UseExceptionHandler()"

[ApiExplorerSettings(IgnoreApi = true)]
[Route(RouteTemplates.ErrorHandlers)]
[ApiController]
[AllowAnonymous]
public class ErrorHandlersController : ControllerBase
{
    [Route(RouteTemplates.DevelopmentErrors)]
    public IActionResult HandleDevelopmentError([FromServices] IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsProduction())
            return NotFound();

        // we can customize pre production error responses

        IExceptionHandlerFeature? exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (exceptionHandlerFeature == null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return Problem(
            detail: exceptionHandlerFeature.Error.StackTrace,
            title: exceptionHandlerFeature.Error.Message,
            instance: null,
            statusCode: null,
            type: null
        );
    }

    [Route(RouteTemplates.Errors)]
    public IActionResult HandleError() => Problem(); // return the default "problem"
}