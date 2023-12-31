using Enterprise.DomainDrivenDesign.Entity.Examples;

namespace Enterprise.DomainDrivenDesign.Repository.Example;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(User user);
}