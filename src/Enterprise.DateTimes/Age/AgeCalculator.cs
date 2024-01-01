
using Enterprise.Calculations;
using Enterprise.DateTimes.Age.Abstract;
using Enterprise.DateTimes.Extensions;
using static Enterprise.DateTimes.Age.AgeCalculationFailureReasons;

namespace Enterprise.DateTimes.Age;

public class AgeCalculator : ICalculateAge
{
    public Calculation<int> CalculateAge(DateTimeOffset birthDate)
    {
        try
        {
            birthDate = birthDate.ToUniversalTime();

            DateTimeOffset today = DateTimeOffset.UtcNow;

            if (birthDate.IsLaterThan(today))
                return Calculation<int>.CanNotBeMadeBecause(BirthDateIsInTheFuture);

            int age = today.Year - birthDate.Year;

            // if the birthday is later in the year (hasn't come to pass yet)
            if (birthDate.Date > today.Date.AddYears(-age))
                age -= 1; // we need to subtract 1

            return Calculation<int>.Success(age);
        }
        catch (Exception ex)
        {
            return Calculation<int>.FailedDueTo(ex);
        }
    }
}