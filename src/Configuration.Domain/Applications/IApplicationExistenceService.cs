namespace Configuration.Domain.Applications;

public interface IApplicationExistenceService
{
    Task<bool> ApplicationExistsAsync(Guid id);
    Task<bool> ApplicationExistsAsync(string uniqueName);
}