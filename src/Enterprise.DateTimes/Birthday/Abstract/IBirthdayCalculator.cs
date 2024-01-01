using Enterprise.Calculations;

namespace Enterprise.DateTimes.Birthday.Abstract;

public interface IBirthdayCalculator
{
    Calculation<int> GetDaysUntilNextBirthday(DateTimeOffset birthDate);
}