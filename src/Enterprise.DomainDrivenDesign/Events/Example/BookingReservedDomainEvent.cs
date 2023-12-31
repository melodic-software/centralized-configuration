using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Enterprise.DomainDrivenDesign.Events.Example
{
    public class BookingReservedDomainEvent : DomainEvent
    {
        public Guid BookingId { get; }

        public BookingReservedDomainEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
