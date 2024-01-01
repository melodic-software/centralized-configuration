using System.Globalization;

namespace Enterprise.DateTimes.Parsing.Abstract;

/// <summary>
/// Provides methods for parsing strings into DateTime and DateTimeOffset objects.
/// </summary>
public interface IDateTimeParser
{
    /// <summary>
    /// Parses the given string into a DateTime object using the specified culture and format. 
    /// The parsed date and time are adjusted to a DateTime object.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <param name="cultureInfo">The culture information to use for parsing. Defaults to CultureInfo.InvariantCulture if null.</param>
    /// <param name="format">The format to use for parsing. Uses default formats if null.</param>
    /// <returns>Parsed DateTime object.</returns>
    DateTime ParseDateTime(string input, CultureInfo? cultureInfo = null, string? format = null);

    /// <summary>
    /// Parses the given string into a DateTime object using the exact specified format.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <param name="format">The exact format to use for parsing. Defaults to 'M/d/yyyy h:mm:ss tt' if null.</param>
    /// <returns>Parsed DateTime object.</returns>
    DateTime ParseExactDateTime(string input, string? format = null);

    /// <summary>
    /// Parses the given string into a DateTimeOffset object using the specified culture and format.
    /// Adjusts the parsed DateTimeOffset object to UTC.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <param name="cultureInfo">The culture information to use for parsing. Defaults to CultureInfo.InvariantCulture if null.</param>
    /// <param name="format">The format to use for parsing. Uses default formats if null.</param>
    /// <returns>Parsed DateTimeOffset object.</returns>
    DateTimeOffset ParseDateTimeOffset(string input, CultureInfo? cultureInfo = null, string? format = null);

    /// <summary>
    /// Parses the given string into a DateTimeOffset object using the exact specified format.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <param name="format">The exact format to use for parsing. Defaults to 'M/d/yyyy h:mm:ss tt' if null.</param>
    /// <returns>Parsed DateTimeOffset object.</returns>
    DateTimeOffset ParseExactDateTimeOffset(string input, string? format = null);
}