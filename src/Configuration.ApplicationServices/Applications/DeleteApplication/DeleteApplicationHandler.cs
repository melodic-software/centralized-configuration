using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using ApplicationId = Configuration.Domain.Applications.ApplicationId;
using Microsoft.Extensions.Logging;
using Enterprise.ApplicationServices.Events;

namespace Configuration.ApplicationServices.Applications.DeleteApplication;

public sealed class DeleteApplicationHandler : CommandHandler<DeleteApplication>
{
    private readonly IApplicationRepository _applicationRepository;

    public DeleteApplicationHandler(IEventServiceFacade eventServiceFacade,
        ILogger<CommandHandler<DeleteApplication>> logger,
        IApplicationRepository applicationRepository) : base(eventServiceFacade, logger)
    {
        _applicationRepository = applicationRepository;
    }

    public override async Task HandleAsync(DeleteApplication command)
    {
        ApplicationId applicationId = new ApplicationId(command.Id);

        Application? application = await _applicationRepository.GetByIdAsync(applicationId);

        if (application == null)
        {
            ApplicationNotFound applicationNotFound = new ApplicationNotFound(applicationId.Value);
            await RaiseEventAsync(applicationNotFound);
            return;
        }

        await _applicationRepository.DeleteApplicationAsync(application.Id);

        ApplicationDeleted applicationDeleted = new ApplicationDeleted(application.Id.Value, application.UniqueName,
            application.Name, application.AbbreviatedName);

        await RaiseEventAsync(applicationDeleted);
    }
}