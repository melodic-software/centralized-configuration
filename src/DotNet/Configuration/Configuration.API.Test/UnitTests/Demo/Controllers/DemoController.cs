using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Test.UnitTests.Demo.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class DemoController : CustomControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        if (!User.IsInRole("Admin") && !User.IsInRole("Special"))
            return Forbid();

        if (User.IsInRole("Special"))
        {
            string actionName = nameof(GetSpecial);

            string controllerName = nameof(DemoController)
                .Replace("Controller", string.Empty);

            return RedirectToAction(actionName, controllerName);
        }

        return Ok();
    }

    [HttpGet]
    public IActionResult GetSpecial()
    {
        return Ok();
    }
}