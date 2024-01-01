using System.Globalization;
using Enterprise.DateTimes.Years.Abstract;

namespace Enterprise.DateTimes.Years;

public class LeapYearCalculator : ILeapYearCalculator
{
    public List<int> GetLeapYears(int startYear, int endYear, Calendar? calendar = null)
    {
        calendar ??= new GregorianCalendar();
        List<int> leapYears = new List<int>();

        for (int year = startYear; year <= endYear; year++)
        {
            if (IsLeapYear(year, calendar))
                leapYears.Add(year);
        }

        return leapYears;
    }

    public bool IsLeapYear(int year, Calendar? calendar = null)
    {
        calendar ??= new GregorianCalendar();
        return calendar.IsLeapYear(year);
    }
}