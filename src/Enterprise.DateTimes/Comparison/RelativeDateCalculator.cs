using Enterprise.Calculations;
using Enterprise.DateTimes.Comparison.Abstract;
using Enterprise.DateTimes.Extensions;
using static Enterprise.DateTimes.Comparison.RelativeDateCalculationFailureReasons;

namespace Enterprise.DateTimes.Comparison;

public class RelativeDateCalculator : IRelativeDateCalculator
{
    public Calculation<TimeSpan> CalculateTimeSince(DateTime pastDate, DateTime referenceDate)
    {
        referenceDate = referenceDate.ToUniversalTime();
        pastDate = pastDate.ToUniversalTime();

        if (pastDate.IsLaterThan(referenceDate))
            return Calculation<TimeSpan>.CanNotBeMadeBecause(FutureDateProvidedForPastCalculation);

        TimeSpan result = referenceDate - pastDate;

        return Calculation<TimeSpan>.Success(result);
    }

    public Calculation<TimeSpan> CalculateTimeUntil(DateTime referenceDate, DateTime futureDate)
    {
        referenceDate = referenceDate.ToUniversalTime();
        futureDate = futureDate.ToUniversalTime();

        if (futureDate.IsEarlierThan(referenceDate))
            return Calculation<TimeSpan>.CanNotBeMadeBecause(PastDateProvidedForFutureCalculation);

        TimeSpan result = futureDate - referenceDate;

        return Calculation<TimeSpan>.Success(result);
    }
}