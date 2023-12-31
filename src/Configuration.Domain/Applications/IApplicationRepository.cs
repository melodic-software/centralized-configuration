namespace Configuration.Domain.Applications;

public interface IApplicationRepository
{
    Task<Application?> GetByIdAsync(Guid id);
    Task<Application?> GetByUniqueNameAsync(string uniqueName);
    Task Save(Application application);
    Task DeleteApplicationAsync(Guid id);
}