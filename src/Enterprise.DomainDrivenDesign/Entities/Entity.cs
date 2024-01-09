using Enterprise.DomainDrivenDesign.Entities.Generic;

namespace Enterprise.DomainDrivenDesign.Entities;

public abstract class Entity : Entity<Guid>
{
    protected Entity(Guid id) : base(id)
    {
    }
}