using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Enterprise.DomainDrivenDesign.Events.Example;

public class BookingReservedDomainEvent : DomainEvent
{
    public BookingReservedDomainEvent(Guid bookingId)
    {
        BookingId = bookingId;
    }

    public Guid BookingId { get; }
}