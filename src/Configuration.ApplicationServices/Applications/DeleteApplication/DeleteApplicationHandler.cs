using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Enterprise.Events.Services.Raising;
using ApplicationId = Configuration.Domain.Applications.ApplicationId;

namespace Configuration.ApplicationServices.Applications.DeleteApplication;

public sealed class DeleteApplicationHandler(
    IRaiseEvents eventRaiser,
    IEventCallbackService eventCallbackService,
    IApplicationRepository applicationRepository)
    : CommandHandler<DeleteApplication>(eventRaiser, eventCallbackService)
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