using AutoMapper;
using Configuration.API.Client.DTOs.Input.V1;
using Configuration.API.Client.DTOs.Output.V1;
using Configuration.API.Routing.Constants;
using Configuration.ApplicationServices.Applications.CreateApplication;
using Configuration.Domain.Applications.Events;
using Enterprise.API.Controllers.Abstract;
using Enterprise.API.ModelBinding;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.DomainDrivenDesign.Event;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Controllers;

[Route(RouteTemplates.ApplicationCollections)]
[ApiController]
public class ApplicationCollectionsController : CustomControllerBase
{
    private readonly ILogger<ApplicationCollectionsController> _logger;
    private readonly IMapper _mapper;

    public ApplicationCollectionsController(ILogger<ApplicationCollectionsController> logger, IMapper mapper)
    {
        // in most cases this will never happen...
        // but it can if the default container is replaced with one that doesn't raise exceptions when an implementation cannot be located
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("({applicationIds})", Name = RouteNames.GetApplicationCollection)]
    public IActionResult Get([ModelBinder(BinderType = typeof(GenericEnumerableModelBinder))] [FromRoute] IEnumerable<Guid> applicationIds) 
    {
        // there's no implicit binding for collection of non primitives types - so we create a custom model binder
        // we could have used a List<string>, but then we'd have to parse them
        // [FromRoute] must be used because [FromBody] is used by default for collection types

        // TODO: implement

        // do we have all requested applications?
        // TODO: check count of applicationIds against query result count
        // return NotFound() if there is a count mismatch

        // TODO: map query model to API model contract

        return Ok();
    }

    [HttpPost(Name = RouteNames.CreateApplicationCollection)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IEnumerable<ApplicationDto>>> Post(IEnumerable<CreateApplicationDto> createApplicationDtos,
        IHandleCommand<CreateApplication> commandHandler)
    {
        // TODO: is this a single transactional use case?
        // do we ever want to allow a situation where some applications are created, and some are not?

        List<ApplicationCreated> applicationCreatedEvents = new List<ApplicationCreated>();
        List<ValidationFailure> validationFailures = new List<ValidationFailure>();

        foreach (CreateApplicationDto createApplicationDto in createApplicationDtos)
            await ExecuteCommand(commandHandler, createApplicationDto, validationFailures, applicationCreatedEvents);

        if (validationFailures.Any())
        {
            validationFailures.ForEach(x => ModelState.AddModelError(x.Context, x.Message));
            return UnprocessableEntity(ModelState);
        }

        List<ApplicationDto> result = applicationCreatedEvents
            .Select(x => _mapper.Map<ApplicationDto>(x))
            .ToList();

        // the resource ID is a collection of application IDs
        string applicationIds = string.Join(",", applicationCreatedEvents.Select(x => x.ApplicationId.ToString()));

        var routeValues = new { applicationIds };
        return CreatedAtRoute(RouteNames.GetApplicationCollection, routeValues, result);
    }

    private async Task ExecuteCommand(IHandleCommand<CreateApplication> commandHandler, CreateApplicationDto createApplicationDto,
        List<ValidationFailure> validationFailures, List<ApplicationCreated> applicationCreatedEvents)
    {
        CreateApplication command = new CreateApplication(createApplicationDto.Id, createApplicationDto.Name, createApplicationDto.AbbreviatedName, createApplicationDto.Description, createApplicationDto.IsActive);

        ApplicationCreated? applicationCreated = null;
        bool validationFailed = false;

        commandHandler.RegisterEventCallback(new Action<ValidationFailure>(validationFailure =>
        {
            validationFailures.Add(validationFailure);

            if (!validationFailed)
                validationFailed = true;
        }));

        commandHandler.RegisterEventCallback(new Action<ApplicationCreated>(@event =>
        {
            applicationCreated = @event;
            applicationCreatedEvents.Add(@event);
        }));

        await commandHandler.HandleAsync(command);

        // TODO: can we handle this better?
        // do we need a separate command?
        commandHandler.ClearCallbacks();

        if (validationFailed || applicationCreated == null)
        {
            // TODO: this can probably be removed, just added it for debugging purposes
            _logger.LogWarning("Application creation failed: {commandName}", command.Name);
        }
    }
}