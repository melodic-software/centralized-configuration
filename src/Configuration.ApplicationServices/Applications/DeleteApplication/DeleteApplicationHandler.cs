using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Events;
using ApplicationId = Configuration.Domain.Applications.ApplicationId;

namespace Configuration.ApplicationServices.Applications.DeleteApplication;

public sealed class DeleteApplicationHandler : CommandHandlerBase<DeleteApplication>
{
    private readonly IApplicationRepository _applicationRepository;

    public DeleteApplicationHandler(IEventServiceFacade eventServiceFacade,
        IApplicationRepository applicationRepository) : base(eventServiceFacade)
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