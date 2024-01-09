namespace Enterprise.DomainDrivenDesign.ValueObjects.Examples.Record;

public record PricingDetails
{
    public PricingDetails(Money PriceForPeriod,
        Money CleaningFee,
        Money AmenitiesUpCharge,
        Money TotalPrice)
    {
        this.PriceForPeriod = PriceForPeriod;
        this.CleaningFee = CleaningFee;
        this.AmenitiesUpCharge = AmenitiesUpCharge;
        this.TotalPrice = TotalPrice;
    }

    public Money PriceForPeriod { get; init; }
    public Money CleaningFee { get; init; }
    public Money AmenitiesUpCharge { get; init; }
    public Money TotalPrice { get; init; }

    public void Deconstruct(out Money PriceForPeriod, out Money CleaningFee, out Money AmenitiesUpCharge, out Money TotalPrice)
    {
        PriceForPeriod = this.PriceForPeriod;
        CleaningFee = this.CleaningFee;
        AmenitiesUpCharge = this.AmenitiesUpCharge;
        TotalPrice = this.TotalPrice;
    }
}