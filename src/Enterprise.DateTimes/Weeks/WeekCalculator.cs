using System.Globalization;
using Enterprise.DateTimes.Weeks.Abstract;

namespace Enterprise.DateTimes.Weeks;

public class WeekCalculator : IWeekCalculator
{
    public int GetISOWeekOfYear(DateTime dateTime)
    {
        return ISOWeek.GetWeekOfYear(dateTime);
    }
    
    public int GetRegionalWeekOfYear(DateTime date, CalendarWeekRule rule, DayOfWeek firstDayOfWeek, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.InvariantCulture;
        Calendar calendar = culture.Calendar;
        return calendar.GetWeekOfYear(date, rule, firstDayOfWeek);
    }
    
    public DateTime GetYearWeekStartDate(int year, int week)
    {
        DateTime januaryFirst = new DateTime(year, 1, 1);
        int daysOffset = DayOfWeek.Monday - januaryFirst.DayOfWeek;

        if (daysOffset > 0)
            daysOffset -= 7;

        DateTime firstMonday = januaryFirst.AddDays(daysOffset);
        int firstWeek = ISOWeek.GetWeekOfYear(firstMonday);

        if (firstWeek <= 1)
            week--;

        DateTime result = firstMonday.AddDays(week * 7);

        return result;
    }

    public int GetTotalISOWeeksInYear(int year)
    {
        DateTime lastDayOfYear = new DateTime(year, 12, 31);

        // Find the last Thursday of the year
        int daysUntilThursday = (int)DayOfWeek.Thursday - (int)lastDayOfYear.DayOfWeek;
        
        if (daysUntilThursday > 0)
            daysUntilThursday -= 7;
        
        DateTime lastThursday = lastDayOfYear.AddDays(daysUntilThursday);
        
        return ISOWeek.GetWeekOfYear(lastThursday);
    }
}