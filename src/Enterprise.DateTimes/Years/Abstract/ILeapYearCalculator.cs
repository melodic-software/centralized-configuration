using System.Globalization;

namespace Enterprise.DateTimes.Years.Abstract;

public interface ILeapYearCalculator
{
    /// <summary>
    /// Calculates all leap years within a specified range for a given calendar system.
    /// </summary>
    /// <param name="startYear">The starting year of the range.</param>
    /// <param name="endYear">The ending year of the range.</param>
    /// <param name="calendar">Optional. The calendar system to use for the calculation. Defaults to GregorianCalendar if null.</param>
    /// <returns>A list of all leap years within the specified range for the given calendar system.</returns>
    public List<int> GetLeapYears(int startYear, int endYear, Calendar? calendar = null);

    /// <summary>
    /// Determines whether the specified year is a leap year, based on the provided calendar system.
    /// </summary>
    /// <remarks>
    /// This method utilizes the IsLeapYear method of the given calendar system to determine leap years.
    /// It supports any calendar that inherits from the System.Globalization.Calendar class.
    /// </remarks>
    /// <param name="year">The year to check for being a leap year.</param>
    /// <param name="calendar">The calendar system to use for the leap year calculation. 
    /// If null, the Gregorian calendar is used as default.</param>
    /// <returns>true if the specified year is a leap year in the given calendar system; otherwise, false.</returns>
    public bool IsLeapYear(int year, Calendar? calendar = null);
}