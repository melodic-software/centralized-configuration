namespace Enterprise.DomainDrivenDesign.Model
{
    /// <summary>
    /// Represents a Value Object in domain-driven design.
    /// Value Objects are immutable and characterized by their attributes rather than a unique identity.
    /// They are used to encapsulate and model the qualitative aspects of the domain.
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public static bool operator ==(ValueObject? a, ValueObject? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b) => !(a == b);

        /// <inheritdoc />
        public bool Equals(ValueObject? other) => !(other is null) && GetAtomicValues().SequenceEqual(other.GetAtomicValues());

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            if (!(obj is ValueObject valueObject))
                return false;

            return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            HashCode hashCode = default;

            foreach (object obj in GetAtomicValues())
                hashCode.Add(obj);

            return hashCode.ToHashCode();
        }

        /// <summary>
        /// Gets the atomic values of the value object.
        /// </summary>
        /// <returns>The collection of objects representing the value object values.</returns>
        protected abstract IEnumerable<object> GetAtomicValues();
    }
}