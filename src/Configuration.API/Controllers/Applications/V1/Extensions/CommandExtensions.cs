using AutoMapper;
using Configuration.API.Client.Models.Input.V1;
using Configuration.API.Client.Models.Output.V1;
using Configuration.API.Routing.Constants;
using Configuration.ApplicationServices.Commands.Applications;
using Configuration.Core.Domain.Model.Events;
using Enterprise.API.Client.Hypermedia;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.DataShaping.Extensions;
using Enterprise.Events.Model.Domain.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace Configuration.API.Controllers.Applications.V1.Extensions;

public static class CommandExtensions
{
    public static async Task<IActionResult> Upsert(this ControllerBase controller, Guid id,
        UpdateApplicationDto updateDto, IHandleCommand<UpdateApplication> updateApplicationHandler,
        IHandleCommand<CreateApplication> createApplicationHandler, IMapper mapper)
    {
        Task<IActionResult> OnNotFound()
        {
            CreateApplicationDto createModel = mapper.Map<CreateApplicationDto>(updateDto);
            createModel.Id = id;
            Task<IActionResult> result = controller.Create(createModel, createApplicationHandler, mapper);
            return result;
        }

        return await controller.Update(id, updateDto, updateApplicationHandler, OnNotFound);
    }

    public static async Task<IActionResult> Create(this ControllerBase controller,
        CreateApplicationDto createDto, IHandleCommand<CreateApplication> commandHandler, IMapper mapper)
    {
        CreateApplication command = new CreateApplication(createDto.Id, createDto.Name, createDto.AbbreviatedName, createDto.Description, createDto.IsActive);

        List<ValidationFailure> validationFailures = new List<ValidationFailure>();
        ApplicationCreated? applicationCreated = null;

        commandHandler.RegisterEventCallback(new Action<ValidationFailure>(validationFailures.Add));
        commandHandler.RegisterEventCallback(new Action<ApplicationCreated>(@event => applicationCreated = @event));
        await commandHandler.HandleAsync(command);

        if (validationFailures.Any())
        {
            validationFailures.ForEach(x => controller.ModelState.AddModelError(x.Context, x.Message));
            return controller.UnprocessableEntity(controller.ModelState);
        }

        // TODO: should we use a 409 conflict if the application (resource) already exists?
        // this means the ID is non-unique (already associated with an existing application)

        ApplicationDto mapResult = mapper.Map<ApplicationDto>(applicationCreated);

        ExpandoObject resultModel = mapResult.ShapeData();

        // add the hypermedia links to the result model
        IEnumerable<HypermediaLinkModel> links = controller.CreateLinksForApplication(mapResult.Id, null);
        IDictionary<string, object?> modelDictionary = resultModel;
        modelDictionary.Add("links", links);

        object? applicationId = modelDictionary[nameof(ApplicationDto.Id)];

        var routeValues = new { id = applicationId };
        IActionResult result = controller.CreatedAtRoute(RouteNames.GetApplicationById, routeValues, resultModel);
        return result;
    }

    public static async Task<IActionResult> Update(this ControllerBase controller, Guid id,
        UpdateApplicationDto updateDto, IHandleCommand<UpdateApplication> commandHandler,
        Func<Task<IActionResult>>? onNotFound = null)
    {
        UpdateApplication command = new UpdateApplication(id, updateDto.Name, updateDto.AbbreviatedName, updateDto.Description, updateDto.IsActive);

        List<ValidationFailure> validationFailures = new List<ValidationFailure>();

        bool notFound = false;

        commandHandler.RegisterEventCallback(new Action<ApplicationNotFound>(@event => notFound = true));
        commandHandler.RegisterEventCallback(new Action<ValidationFailure>(validationFailures.Add));
        await commandHandler.HandleAsync(command);

        if (notFound)
        {
            if (onNotFound != null)
                return await onNotFound.Invoke();

            return controller.NotFound();
        }

        if (validationFailures.Any())
        {
            // TODO: return validation failures to the client
            return controller.UnprocessableEntity();
        }

        // TODO: should we return this if there is a concurrency issue?
        //return controller.Conflict();

        return controller.NoContent();
    }
}