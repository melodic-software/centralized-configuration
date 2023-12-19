using Configuration.Core.Domain.Model.Entities;

namespace Configuration.Core.Domain.Services.Abstract.Repositories;

public interface IApplicationRepository
{
    Task<Application?> GetByIdAsync(Guid id);
    Task<Application?> GetByUniqueNameAsync(string uniqueName);
    Task Save(Application application);
    Task DeleteApplicationAsync(Guid id);
}