using Configuration.API.Routing.Constants;
using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Controllers;
// NOTE: this is just for testing error handling responses

[Route(RouteTemplates.Exceptions)]
[ApiController]
public class ExceptionsController : CustomControllerBase
{
    [HttpPost(Name = RouteNames.GenerateException)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post()
    {
        int x = 0;
        int y = 10;

        int divideByZeroResult = y / x;

        return StatusCode(StatusCodes.Status201Created);
    }
}