using Enterprise.DomainDrivenDesign.Entities.Examples;

namespace Enterprise.DomainDrivenDesign.Repositories.Example;

public interface IApartmentRepository
{
    Task<Apartment> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}