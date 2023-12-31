namespace Enterprise.DomainDrivenDesign.ValueObject.Examples.Record;

public record PricingDetails(
    Money PriceForPeriod,
    Money CleaningFee,
    Money AmenitiesUpCharge,
    Money TotalPrice);