using Configuration.API.Client.DTOs.Input.V1;
using Configuration.API.Client.DTOs.Output.V1;
using Configuration.API.InMemory;
using Configuration.API.Routing;
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

        EnvironmentDto? result = ConfigurationDataStore.Environments
            .FirstOrDefault(x => x.EnvironmentId == environmentId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet(Name = RouteNames.GetEnvironments)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetEnvironments([FromQuery] GetEnvironmentsDto queryParams)
    {
        if (queryParams.EnvironmentId.HasValue)
            return GetEnvironmentById(queryParams.EnvironmentId.Value);

        if (!string.IsNullOrWhiteSpace(queryParams.EnvironmentUniqueName))
        {
            EnvironmentDto? environment = ConfigurationDataStore.Environments
                .FirstOrDefault(x => x.EnvironmentUniqueName == queryParams.EnvironmentUniqueName);

            if (environment == null)
                return NotFound();

            return Ok(environment);
        }

        List<EnvironmentDto> allEnvironments = ConfigurationDataStore.Environments;

        return Ok(allEnvironments);
    }
}