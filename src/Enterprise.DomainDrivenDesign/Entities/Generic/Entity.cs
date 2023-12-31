﻿using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Enterprise.DomainDrivenDesign.Entities.Generic;

public abstract class Entity<TId> : IEntity, IEquatable<Entity<TId>> where TId : IEquatable<TId>
{
    protected readonly List<IDomainEvent> DomainEvents = new();

    public TId Id { get; init; }

    protected Entity(TId id)
    {
        if (Equals(id, default(TId)))
            throw new ArgumentException("The ID cannot be the default value.", nameof(id));

        Id = id;
    }
        
    protected void RecordDomainEvent(IDomainEvent domainEvent)
    {
        bool eventAlreadyAdded = DomainEvents.Any(x => x.Id == domainEvent.Id);

        if (eventAlreadyAdded)
            return;

        DomainEvents.Add(domainEvent);
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return DomainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        DomainEvents.Clear();
    }

    public bool Equals(Entity<TId>? other)
    {
        return other != null && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TId>);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return this.GetType().Name;
    }
}