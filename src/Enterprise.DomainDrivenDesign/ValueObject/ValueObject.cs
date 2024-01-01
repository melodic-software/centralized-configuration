namespace Enterprise.DomainDrivenDesign.ValueObject;

/// <summary>
/// An object that represents a descriptive aspect of the domain with no conceptual identity.
/// Value Objects are instantiated to represent elements of the design that we care about only for what they are, not who or which they are.
/// They are different from entities in that they don't have a concept of identity. They encapsulate primitive types in the domain and solve primitive obsession.
/// Another quality of value objects is structural equality. Two value objects are equal if their values are the same.
/// They are immutable.
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

    public static bool operator !=(ValueObject? a, ValueObject? b)
    {
        return !(a == b);
    }

    public virtual bool Equals(ValueObject? other)
    {
        if (other == null)
            return false;

        var areEqual = ValuesAreEqual(other);

        return areEqual;
    }

    public override bool Equals(object? obj)
    {
        bool areEqual = false;

        if (obj is ValueObject valueObject)
            areEqual = ValuesAreEqual(valueObject);

        return areEqual;
    }

    /// <summary>
    /// Generates a hash code based on the atomic values of the value object.
    /// </summary>
    public override int GetHashCode()
    {
        IEnumerable<object> atomicValues = GetAtomicValues();

        int result = default(int);

        foreach (object? value in atomicValues)
        {
            int valueHashCode = value.GetHashCode();
            result = HashCode.Combine(result, valueHashCode);
        }

        return result;
    }

    protected abstract IEnumerable<object> GetAtomicValues();

    private bool ValuesAreEqual(ValueObject valueObject)
    {
        IEnumerable<object> atomicValues = GetAtomicValues();
        IEnumerable<object> otherAtomicValues = valueObject.GetAtomicValues();
        bool areEqual = atomicValues.SequenceEqual(otherAtomicValues);
        return areEqual;
    }
}