using Enterprise.DomainDrivenDesign.Event.Abstract;

namespace Enterprise.DomainDrivenDesign.Event.Example
{
    public class BookingReservedDomainEvent(Guid bookingId) : DomainEvent
    {
        public Guid BookingId { get; } = bookingId;
    }
}
