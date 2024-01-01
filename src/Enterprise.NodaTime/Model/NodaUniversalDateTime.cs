using Enterprise.DateTimes.Model;
using Enterprise.NodaTime.Constants;
using NodaTime;
using NodaTime.Extensions;

namespace Enterprise.NodaTime.Model;

/// <summary>
/// Represents a date and time in Universal Coordinated Time (UTC) using NodaTime.
/// Inherits functionality from UniversalDateTime and extends it to work with NodaTime types.
/// </summary>
public class NodaUniversalDateTime : UniversalDateTime
{
    /// <summary>
    /// Gets the Instant representing the moment in UTC.
    /// </summary>
    public Instant Instant { get; private set; }

    /// <summary>
    /// Initializes a new instance of the NodaUniversalDateTime class set to the current UTC date and time.
    /// </summary>
    public NodaUniversalDateTime()
    {
        SystemClock systemClock = SystemClock.Instance;
        ZonedClock utcSystemClock = systemClock.InUtc();
        Instant currentUtcInstant = utcSystemClock.GetCurrentInstant();

        Instant = currentUtcInstant;
        DateTimeOffset = currentUtcInstant.ToDateTimeOffset();
    }

    private NodaUniversalDateTime(Instant instant)
    {
        Instant = instant;
        DateTimeOffset = instant.ToDateTimeOffset();
    }

    /// <summary>
    /// Creates a NodaUniversalDateTime from a ZonedDateTime, converting it to UTC if necessary.
    /// </summary>
    /// <param name="zonedDateTime">The ZonedDateTime to convert.</param>
    /// <returns>A NodaUniversalDateTime instance representing the same point in time in UTC.</returns>
    /// <exception cref="ArgumentNullException">Thrown if zonedDateTime is null.</exception>
    public static NodaUniversalDateTime From(ZonedDateTime zonedDateTime)
    {
        Offset offset = zonedDateTime.Offset;
        bool isZeroOffset = offset.Ticks == 0;

        DateTimeZone zone = zonedDateTime.Zone;
        bool isUtcZone = zone.Id == TimeZoneDatabaseIds.Utc;

        if (isZeroOffset || isUtcZone)
        {
            Instant instant = zonedDateTime.ToInstant();
            return new NodaUniversalDateTime(instant);
        }

        Instant zonedInstant = zonedDateTime.ToInstant();
        ZonedDateTime utcZoned = zonedInstant.InUtc();
        Instant utcInstant = utcZoned.ToInstant();

        return new NodaUniversalDateTime(utcInstant);
    }
}