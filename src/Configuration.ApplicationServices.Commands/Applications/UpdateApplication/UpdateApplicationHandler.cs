﻿using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.DomainDrivenDesign.Events;
using ApplicationId = Configuration.Domain.Applications.ApplicationId;

namespace Configuration.ApplicationServices.Commands.Applications.UpdateApplication;

public sealed class UpdateApplicationHandler(
    IApplicationServiceDependencies appServiceDependencies,
    IApplicationExistenceService applicationExistenceService,
    ApplicationValidationService applicationValidationService,
    IApplicationRepository applicationRepository)
    : CommandHandler<UpdateApplication>(appServiceDependencies)
{
    public override async Task HandleAsync(UpdateApplication command)
    {
        ApplicationId applicationId = new ApplicationId(command.Id);

        bool applicationExists = await applicationExistenceService.ApplicationExistsAsync(applicationId);

        if (!applicationExists)
        {
            // TODO: throw ApplicationNotFoundException?
            ApplicationNotFound applicationNotFound = new ApplicationNotFound(applicationId.Value);
            await RaiseEventAsync(applicationNotFound);
            return;
        }

        Application? application = await applicationRepository.GetByIdAsync(applicationId);

        if (application == null)
        {
            // TODO: throw ApplicationNotFoundException?
            ApplicationNotFound applicationNotFound = new ApplicationNotFound(applicationId.Value);
            await RaiseEventAsync(applicationNotFound);
            return;
        }

        application.Update(command.Name, command.AbbreviatedName, command.Description, command.IsActive);

        List<ValidationFailure> validationFailures = applicationValidationService.Validate(application);

        if (validationFailures.Any())
        {
            await RaiseEventsAsync(validationFailures);
            return;
        }

        await applicationRepository.Save(application);

        ApplicationUpdated applicationUpdated = new ApplicationUpdated(
            application.Id.Value,
            application.UniqueName,
            application.Name,
            application.AbbreviatedName,
            application.IsActive
        );

        await RaiseEventAsync(applicationUpdated);
    }
}