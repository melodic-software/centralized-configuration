namespace Enterprise.Core.Abstractions;

/// <summary>
/// This is a marker interface for soft-deletable entities.
/// </summary>
public interface ISoftDeletableEntity
{
    /// <summary>
    /// Gets the date and time that the entity was deleted.
    /// </summary>
    DateTime? DateDeleted { get; }

    /// <summary>
    /// Gets a value indicating whether the entity has been deleted.
    /// </summary>
    bool IsDeleted { get; }
}