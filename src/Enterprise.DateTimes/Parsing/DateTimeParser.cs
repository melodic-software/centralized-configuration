using System.Globalization;
using Enterprise.DateTimes.Parsing.Abstract;

namespace Enterprise.DateTimes.Parsing;

// NOTE: the default behavior for DateTime.Parse() results in a translation of the date and time to the local time zone.
// This is usually not the desired behavior, so we translate everything to UTC.

/// <summary>
/// A class for parsing date and time strings into DateTime and DateTimeOffset objects.
/// </summary>
public class DateTimeParser : IDateTimeParser
{
    /// <inheritdoc />
    public DateTime ParseDateTime(string input, CultureInfo? cultureInfo = null, string? format = null)
    {
        return ParseDateTimeOffset(input, cultureInfo, format).DateTime;
    }

    /// <inheritdoc />
    public DateTime ParseExactDateTime(string input, string? format = null)
    {
        return ParseExactDateTimeOffset(input, format).DateTime;
    }

    /// <inheritdoc />
    public DateTimeOffset ParseDateTimeOffset(string input, CultureInfo? cultureInfo = null, string? format = null)
    {
        cultureInfo ??= CultureInfo.InvariantCulture;
        DateTimeStyles dateTimeStyles = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal;

        // Attempts to parse the input string to a DateTimeOffset object.
        if (!string.IsNullOrEmpty(format))
        {
            if (DateTimeOffset.TryParseExact(input, format, cultureInfo, dateTimeStyles, out DateTimeOffset result))
                return result;
        }
        else
        {
            if (DateTimeOffset.TryParse(input, cultureInfo, dateTimeStyles, out DateTimeOffset result))
                return result;
        }

        throw new FormatException("Invalid date format.");
    }

    /// <inheritdoc />
    public DateTimeOffset ParseExactDateTimeOffset(string input, string? format = null)
    {
        format ??= "M/d/yyyy h:mm:ss tt";
        DateTimeStyles dateTimeStyles = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal;

        // Attempts to parse the input string to a DateTimeOffset object using the exact format specified.
        if (DateTimeOffset.TryParseExact(input, format, CultureInfo.InvariantCulture, dateTimeStyles, out DateTimeOffset result))
            return result;
        
        throw new FormatException("Invalid date format.");
    }
}
