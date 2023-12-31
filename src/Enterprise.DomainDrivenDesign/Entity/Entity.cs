using Enterprise.DomainDrivenDesign.Entity.Generic;

namespace Enterprise.DomainDrivenDesign.Entity
{
    public abstract class Entity(Guid id) : Entity<Guid>(id);
}
