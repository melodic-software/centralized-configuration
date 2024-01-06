using Enterprise.DomainDrivenDesign.DomainServices.Examples;
using Enterprise.DomainDrivenDesign.Enums.Examples;
using Enterprise.DomainDrivenDesign.Events.Example;
using Enterprise.DomainDrivenDesign.ValueObjects.Examples.Record;

namespace Enterprise.DomainDrivenDesign.Entities.Examples;

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
        DateTime dateCreated)
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
        DateCreated = dateCreated;
    }

    public Guid ApartmentId { get; private set; }
    public Guid UserId { get; private set; }
    public DateRange Duration { get; private set; }
    public Money PriceForPeriod { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money AmenitiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime DateCreated { get; private set; }

    public static Booking Reserve(Apartment apartment, Guid userId, DateRange duration, DateTime utcNow, PricingService pricingService)
    {
        PricingDetails pricingDetails = pricingService.CalculatePrice(apartment, duration);

        Booking booking = new Booking(
            Guid.NewGuid(),
            apartment.Id,
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

        apartment.DateLastBooked = utcNow;

        return booking;
    }
}