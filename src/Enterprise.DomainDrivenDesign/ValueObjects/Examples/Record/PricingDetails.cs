namespace Enterprise.DomainDrivenDesign.ValueObjects.Examples.Record;

public record PricingDetails(
    Money PriceForPeriod,
    Money CleaningFee,
    Money AmenitiesUpCharge,
    Money TotalPrice);