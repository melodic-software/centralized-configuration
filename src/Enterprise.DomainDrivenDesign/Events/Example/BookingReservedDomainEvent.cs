using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Enterprise.DomainDrivenDesign.Events.Example;

public class BookingReservedDomainEvent(Guid bookingId) : DomainEvent
{
    public Guid BookingId { get; } = bookingId;
}