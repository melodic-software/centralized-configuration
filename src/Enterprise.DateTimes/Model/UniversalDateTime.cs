using Enterprise.DateTimes.Extensions;
using static Enterprise.DateTimes.Formatting.DateTimeFormatStrings;

namespace Enterprise.DateTimes.Model;

/// <summary>
/// Represents a date and time in Universal Coordinated Time (UTC).
/// NOTE: UTC is a standard, and GMT is a timezone.
/// </summary>
public class UniversalDateTime
{
    /// <summary>
    /// Initializes a new instance of the UniversalDateTime class set to the current UTC date and time.
    /// </summary>
    public UniversalDateTime() : this(DateTimeOffset.UtcNow)
    {
    }

    /// <summary>
    /// Initializes a new instance of the UniversalDateTime class using the specified DateTimeOffset.
    /// </summary>
    /// <param name="dateTimeOffset">The DateTimeOffset to use. Must be in UTC.</param>
    /// <exception cref="ArgumentException">Thrown if the provided DateTimeOffset is not in UTC.</exception>
    public UniversalDateTime(DateTimeOffset dateTimeOffset)
    {
        if (dateTimeOffset.Offset != TimeSpan.Zero)
            throw new ArgumentException("DateTimeOffset must be in UTC.", nameof(dateTimeOffset));

        DateTimeOffset = dateTimeOffset;
    }

    /// <summary>
    /// Gets the DateTimeOffset in UTC.
    /// </summary>
    public DateTimeOffset DateTimeOffset { get; protected set; }

    /// <summary>
    /// Gets the DateTime representation of the DateTimeOffset in UTC.
    /// </summary>
    public DateTime DateTime
    {
        get
        {
            DateTime dateTime = DateTimeOffset.UtcDateTime;
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            return dateTime;
        }
    }

    /// <summary>
    /// Gets the DateOnly representation of the DateTimeOffset.
    /// </summary>
    public DateOnly DateOnly => DateTime.DateOnly();

    /// <summary>
    /// Returns a string that represents the current object in ISO 8601 format.
    /// </summary>
    /// <returns>A string in ISO 8601 format representing the current UTC date and time.</returns>
    public override string ToString()
    {
        return DateTimeOffset.ToString(Iso8601);
    }
}