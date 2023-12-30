namespace Enterprise.DomainDrivenDesign.Entity.Generic
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : IEquatable<TId>
    {
        public TId Id { get; init; }

        protected Entity(TId id)
        {
            if (Equals(id, default(TId)))
                throw new ArgumentException("The ID cannot be the default value.", nameof(id));

            Id = id;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Entity<TId>);
        }

        public bool Equals(Entity<TId>? other)
        {
            return other != null && Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}