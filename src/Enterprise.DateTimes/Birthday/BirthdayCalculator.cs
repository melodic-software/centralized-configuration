using Enterprise.Calculations;
using Enterprise.DateTimes.Birthday.Abstract;
using Enterprise.DateTimes.Extensions;
using static Enterprise.DateTimes.Birthday.BirthdayCalculationFailureReasons;

namespace Enterprise.DateTimes.Birthday;

public class BirthdayCalculator : IBirthdayCalculator
{
    public Calculation<int> GetDaysUntilNextBirthday(DateTimeOffset birthDate)
    {
        birthDate = birthDate.ToUniversalTime();

        DateTimeOffset today = DateTimeOffset.UtcNow.Date; // strip out the time portion

        if (birthDate.IsLaterThan(today))
            return Calculation<int>.CanNotBeMadeBecause(BirthDateIsInTheFuture);

        DateTimeOffset birthday = new DateTimeOffset(today.Year, birthDate.Month, 01, 0, 0, 0, TimeSpan.Zero);

        // handle leap days
        birthday = birthday.AddDays(birthDate.Day - 1);

        if (birthday < today)
        {
            birthday = new DateTime(today.Year + 1, birthDate.Month, 01);

            // handle leap days
            birthday = birthday.AddDays(birthDate.Day - 1);
        }

        int totalDays = (int)(birthday - today).TotalDays;

        return Calculation<int>.Success(totalDays);
    }
}