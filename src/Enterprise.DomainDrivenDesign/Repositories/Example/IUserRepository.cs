using Enterprise.DomainDrivenDesign.Entities.Examples;

namespace Enterprise.DomainDrivenDesign.Repositories.Example;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(User user);
}