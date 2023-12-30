namespace Enterprise.Core.Abstractions;

/// <summary>
/// This is a marker interface for auditable entities.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Gets the creation date and time.
    /// </summary>
    DateTime DateCreated { get; }

    /// <summary>
    /// Gets the modified date and time.
    /// </summary>
    DateTime? DateModified { get; }
}