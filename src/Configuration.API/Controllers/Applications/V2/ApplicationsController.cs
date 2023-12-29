using Asp.Versioning;
using AutoMapper;
using Configuration.API.Client.Models.Output.V2;
using Configuration.API.Routing.Constants;
using Configuration.ApplicationServices.Queries.Applications;
using Configuration.Core.Queries.Model;
using Enterprise.API.Controllers.Abstract;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Controllers.Applications.V2;
// NOTE: this versioned controller was created to demonstrate how versioning can be handled using a separate namespaced controller
// this usually involves a specific set of versioned input and output models to isolate breaking changes to other previous versions and versions to come
// in most cases, versioned application services and/or core domain objects and services may be required as well

// one alternative to versioning resources is to use vendor media types, but it results in more actions on a single controller
// although these can be split by creating partial classes if it becomes too unwieldy

[Route(RouteTemplates.Applications)]
[ApiController]
[ApiVersion("2.0")]
[ControllerName("Applications")]
public class ApplicationsControllerV2 : CustomControllerBase
{
    private readonly IMapper _mapper;

    public ApplicationsControllerV2(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Get an application by ID. (Version 2)
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /applications/500f86a2-65f7-4fc2-836a-2b14f8686209
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "applicationId": "500f86a2-65f7-4fc2-836a-2b14f8686209",
    ///         "uniqueName": "Demo Application-500f86a2",
    ///         "name": "Demo Application",
    ///         "isActive": true
    ///     }
    /// 
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="queryHandler"></param>
    /// <returns>an IActionResult</returns>
    /// <response code="200">Returns the requested application.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Application not found.</response>
    [HttpGet("{id:guid}", Name = RouteNames.GetApplicationById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces(typeof(ApplicationDto))] // this is the "V2" version of the model
    public async Task<ActionResult<ApplicationDto>> GetApplicationById(Guid id, [FromServices] IHandleQuery<GetApplicationById, Application?> queryHandler)
    {
        // the data shaping support has been stripped to keep this as a simple self-contained example
        // technically we could also use a versioned application service (query object, and result) if needed

        GetApplicationById query = new GetApplicationById(id);

        Application? application = await queryHandler.HandleAsync(query);

        if (application == null)
            return NotFound();

        // normally we'd use auto mapper here to map from the query object to the specific API model (v2) contract
        ApplicationDto result = _mapper.Map<ApplicationDto>(application);

        return Ok(result);
    }
}