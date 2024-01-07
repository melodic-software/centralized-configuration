namespace Configuration.Domain.Applications;

public interface IApplicationExistenceService
{
    Task<bool> ApplicationExistsAsync(ApplicationId id);
    Task<bool> ApplicationExistsAsync(string uniqueName);
}