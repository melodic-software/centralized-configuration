using Enterprise.DomainDrivenDesign.Entity.Examples;

namespace Enterprise.DomainDrivenDesign.Repository.Example;

public interface IApartmentRepository
{
    Task<Apartment> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}