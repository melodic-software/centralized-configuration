using Enterprise.DomainDrivenDesign.Entity.Generic;
using Enterprise.DomainDrivenDesign.Enum.Examples;
using Enterprise.DomainDrivenDesign.Events.Example;
using Enterprise.DomainDrivenDesign.ValueObject.Examples.Record;

namespace Enterprise.DomainDrivenDesign.Entity.Examples;

public sealed class Booking : Entity
{
    private Booking(Guid id,
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        Money priceForPeriod,
        Money cleaningFee,
        Money amenitiesUpCharge,
        Money totalPrice,
        BookingStatus status,
        DateTime createdOnUtc)
        : base(id)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        Status = status;
        CreatedOnUtc = createdOnUtc;
    }

    public Guid ApartmentId { get; private set; }
    public Guid UserId { get; private set; }
    public DateRange Duration { get; private set; }
    public Money PriceForPeriod { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money AmenitiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    public static Booking Reserve(Guid apartmentId, Guid userId, DateRange duration, DateTime utcNow, PricingDetails pricingDetails)
    {
        var booking = new Booking(
            Guid.NewGuid(),
            apartmentId,
            userId,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpCharge,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved,
            utcNow);

        BookingReservedDomainEvent bookingReserved = new BookingReservedDomainEvent(booking.Id);

        booking.AddDomainEvent(bookingReserved);

        return booking;
    }
}