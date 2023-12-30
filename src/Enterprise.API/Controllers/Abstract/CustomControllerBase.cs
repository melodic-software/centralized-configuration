using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enterprise.API.Controllers.Abstract;

// TODO: might need to create a secondary base class or apply this at individual controllers at the API instance level
[ProducesResponseType(StatusCodes.Status401Unauthorized)] 
public class CustomControllerBase : ControllerBase
{
    public override ActionResult ValidationProblem(ModelStateDictionary modelStateDictionary)
    {
        IOptions<ApiBehaviorOptions> options = HttpContext.RequestServices
            .GetRequiredService<IOptions<ApiBehaviorOptions>>();

        // here we have access to the response factory, which can be set in configuration to customize problem details responses
        ActionContext actionContext = ControllerContext;
        Func<ActionContext, IActionResult> invalidModelStateResponseFactory = options.Value.InvalidModelStateResponseFactory;
        IActionResult actionResult = invalidModelStateResponseFactory(actionContext);

        ActionResult result = (ActionResult)actionResult;

        return result;
    }
}