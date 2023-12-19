using Configuration.API.Client.Models.Input.V1;
using Configuration.API.Client.Models.Output.V1;
using Configuration.API.InMemory;
using Configuration.API.Routing.Constants;
using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Controllers;

[Route(RouteTemplates.Environments)]
[ApiController]
public class EnvironmentsController : CustomControllerBase
{
    [HttpGet("{environmentId:guid}", Name = RouteNames.GetEnvironmentById)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetEnvironmentById(Guid environmentId)
    {
        if (environmentId == Guid.Empty)
            return BadRequest();

        EnvironmentModel? environment = ConfigurationDataStore.Environments
            .FirstOrDefault(x => x.EnvironmentId == environmentId);

        if (environment == null)
            return NotFound();

        return Ok(environment);
    }

    [HttpGet(Name = RouteNames.GetEnvironments)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetEnvironments([FromQuery] GetEnvironmentsModel model)
    {
        if (model.EnvironmentId.HasValue)
            return GetEnvironmentById(model.EnvironmentId.Value);

        if (!string.IsNullOrWhiteSpace(model.EnvironmentUniqueName))
        {
            EnvironmentModel? environment = ConfigurationDataStore.Environments
                .FirstOrDefault(x => x.EnvironmentUniqueName == model.EnvironmentUniqueName);

            if (environment == null)
                return NotFound();

            return Ok(environment);
        }

        List<EnvironmentModel> result = ConfigurationDataStore.Environments;

        return Ok(result);
    }
}