using System.Globalization;

namespace Enterprise.DateTimes.Weeks.Abstract;

/// <summary>
/// Provides services for calculating week numbers and dates based on different standards.
/// This includes both ISO 8601 week calculations and regional week calculations,
/// as well as methods to get the start date of a week and the total number of weeks in a year.
/// </summary>
public interface IWeekCalculator
{
    /// <summary>
    /// Calculates the ISO 8601 week number for a given date.
    /// </summary>
    /// <remarks>
    /// The ISO 8601 week date system is widely used in many parts of the world and is the international standard.
    /// According to this standard:
    /// - A week starts on Monday.
    /// - The first week of the year is the week that contains the first Thursday of the year.
    /// This method is preferred for applications needing standardization across different regions,
    /// particularly useful in international contexts for consistency.
    /// </remarks>
    /// <param name="dateTime">The DateTime for which the week number is to be calculated.</param>
    /// <returns>The ISO 8601 week number of the year.</returns>
    public int GetISOWeekOfYear(DateTime dateTime);

    /// <summary>
    /// Calculates the week number for a given date based on specific cultural or regional settings.
    /// </summary>
    /// <remarks>
    /// This method is suitable for applications tailored to specific cultures or regions where the
    /// definition of the first day of the week and the first week of the year may vary.
    /// The CalendarWeekRule and DayOfWeek parameters can be adjusted to match local conventions.
    /// </remarks>
    /// <param name="date">The DateTime for which the week number is to be calculated.</param>
    /// <param name="rule">Defines the rule used to determine the first week of the year.</param>
    /// <param name="firstDayOfWeek">Specifies the first day of the week.</param>
    /// <param name="culture">Optional. The culture to use for the calculation. If null, InvariantCulture is used.</param>
    /// <returns>The week number of the year based on the specified culture or region.</returns>
    public int GetRegionalWeekOfYear(DateTime date, CalendarWeekRule rule, DayOfWeek firstDayOfWeek, CultureInfo? culture = null);

    /// <summary>
    /// Gets the start date of a given year and ISO week number.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="week">The ISO week number.</param>
    /// <returns>The DateTime representing the start date of the given ISO week.</returns>
    public DateTime GetYearWeekStartDate(int year, int week);

    /// <summary>
    /// Calculates the total number of ISO weeks in a given year.
    /// </summary>
    /// <param name="year">The year for which the number of weeks is to be calculated.</param>
    /// <returns>The total number of ISO weeks in the specified year.</returns>
    public int GetTotalISOWeeksInYear(int year);
}