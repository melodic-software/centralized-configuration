namespace Configuration.Core.Domain.Services.Abstract;

public interface IApplicationExistenceService
{
    Task<bool> ApplicationExistsAsync(Guid id);
    Task<bool> ApplicationExistsAsync(string uniqueName);
}