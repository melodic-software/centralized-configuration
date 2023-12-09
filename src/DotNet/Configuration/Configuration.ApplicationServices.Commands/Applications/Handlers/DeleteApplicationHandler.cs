﻿using Configuration.Core.Domain.Model.Entities;
using Configuration.Core.Domain.Model.Events;
using Configuration.Core.Domain.Services.Abstract.Repositories;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers;

namespace Configuration.ApplicationServices.Commands.Applications.Handlers;

public class DeleteApplicationHandler : CommandHandler<DeleteApplication>
{
    private readonly IApplicationRepository _applicationRepository;

    public DeleteApplicationHandler(IApplicationServiceDependencies applicationServiceDependencies,
        IApplicationRepository applicationRepository) : base(applicationServiceDependencies)
    {
        _applicationRepository = applicationRepository;
    }

    public override async Task HandleAsync(DeleteApplication command)
    {
        Application? application = await _applicationRepository.GetByIdAsync(command.Id);

        if (application == null)
        {
            ApplicationNotFound applicationNotFound = new ApplicationNotFound(command.Id);
            await RaiseEventAsync(applicationNotFound);
            return;
        }

        await _applicationRepository.DeleteApplicationAsync(application.Id);

        ApplicationDeleted applicationDeleted = new ApplicationDeleted(application.Id, application.UniqueName,
            application.Name, application.AbbreviatedName);

        await RaiseEventAsync(applicationDeleted);
    }
}