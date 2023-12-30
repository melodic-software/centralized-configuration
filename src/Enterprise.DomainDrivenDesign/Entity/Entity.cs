using Enterprise.DomainDrivenDesign.Entity.Generic;

namespace Enterprise.DomainDrivenDesign.Entity
{
    public abstract class Entity : Entity<Guid>
    {
        protected Entity(Guid id) : base(id)
        {
        }
    }
}
