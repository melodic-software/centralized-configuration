namespace Configuration.Domain.Applications;

public interface IApplicationRepository
{
    Task<Application?> GetByIdAsync(ApplicationId id);
    Task<Application?> GetByUniqueNameAsync(string uniqueName);
    Task Save(Application application);
    Task DeleteApplicationAsync(ApplicationId id);
}