using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using ApplicationId = Configuration.Domain.Applications.ApplicationId;

namespace Configuration.ApplicationServices.Commands.Applications.DeleteApplication;

public sealed class DeleteApplicationHandler(
    IApplicationServiceDependencies appServiceDependencies,
    IApplicationRepository applicationRepository)
    : CommandHandler<DeleteApplication>(appServiceDependencies)
{
    public override async Task HandleAsync(DeleteApplication command)
    {
        ApplicationId applicationId = new ApplicationId(command.Id);

        Application? application = await applicationRepository.GetByIdAsync(applicationId);

        if (application == null)
        {
            ApplicationNotFound applicationNotFound = new ApplicationNotFound(applicationId.Value);
            await RaiseEventAsync(applicationNotFound);
            return;
        }

        await applicationRepository.DeleteApplicationAsync(application.Id);

        ApplicationDeleted applicationDeleted = new ApplicationDeleted(application.Id.Value, application.UniqueName,
            application.Name, application.AbbreviatedName);

        await RaiseEventAsync(applicationDeleted);
    }
}