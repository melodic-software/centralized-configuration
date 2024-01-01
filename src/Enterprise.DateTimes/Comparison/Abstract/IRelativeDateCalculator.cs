using Enterprise.Calculations;

namespace Enterprise.DateTimes.Comparison.Abstract;

public interface IRelativeDateCalculator
{
    Calculation<TimeSpan> CalculateTimeSince(DateTime pastDate, DateTime referenceDate);
    Calculation<TimeSpan> CalculateTimeUntil(DateTime referenceDate, DateTime futureDate);
}