using Enterprise.DomainDrivenDesign.Entities.Generic;

namespace Enterprise.DomainDrivenDesign.Entities;

public abstract class Entity(Guid id) : Entity<Guid>(id);