using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;

namespace Configuration.ApplicationServices.Commands.Applications.Handlers;

public sealed class DeleteApplicationHandler(
    IApplicationServiceDependencies appServiceDependencies,
    IApplicationRepository applicationRepository)
    : CommandHandler<DeleteApplication>(appServiceDependencies)
{
    public override async Task HandleAsync(DeleteApplication command)
    {
        Application? application = await applicationRepository.GetByIdAsync(command.Id);

        if (application == null)
        {
            ApplicationNotFound applicationNotFound = new ApplicationNotFound(command.Id);
            await RaiseEventAsync(applicationNotFound);
            return;
        }

        await applicationRepository.DeleteApplicationAsync(application.Id);

        ApplicationDeleted applicationDeleted = new ApplicationDeleted(application.Id, application.UniqueName,
            application.Name, application.AbbreviatedName);

        await RaiseEventAsync(applicationDeleted);
    }
}