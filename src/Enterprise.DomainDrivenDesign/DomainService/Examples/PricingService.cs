using Enterprise.DomainDrivenDesign.Entity.Examples;
using Enterprise.DomainDrivenDesign.Enum.Examples;
using Enterprise.DomainDrivenDesign.ValueObject.Examples.Record;

namespace Enterprise.DomainDrivenDesign.DomainService.Examples;

public class PricingService
{
    public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
    {
        Currency currency = apartment.Price.Currency;

        Money priceForPeriod = new Money(
            apartment.Price.Amount * period.LengthInDays,
            currency
        );

        decimal percentageUpCharge = 0;

        foreach (Amenity apartmentAmenity in apartment.Amenities)
        {
            percentageUpCharge += apartmentAmenity switch
            {
                Amenity.GardenView or Amenity.MountainView => 0.05m,
                Amenity.AirConditioning => 0.01m,
                Amenity.Parking => 0.01m,
                _ => 0
            };
        }

        Money amenitiesUpCharge = Money.Zero();
        if (percentageUpCharge > 0)
        {
            amenitiesUpCharge = new Money(
                priceForPeriod.Amount * percentageUpCharge, 
                currency);
        }

        Money totalPrice = Money.Zero();

        totalPrice += priceForPeriod;

        if (!apartment.CleaningFee.IsZero())
        {
            totalPrice += apartment.CleaningFee;
        }

        totalPrice += amenitiesUpCharge;

        return new PricingDetails(priceForPeriod, apartment.CleaningFee, amenitiesUpCharge, totalPrice);
    }
}