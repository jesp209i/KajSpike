using KajSpike.Domain.ValueObjects;
using KajSpike.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace KajSpike.Domain
{
    public class Booking : Entity<BookingId>
    {

        public CalendarId ParentId { get; private set; }
        public TimeRange TimeRange { get; private set; }

        public BookingBookedBy BookedBy { get; private set; }
        public DateTimeOffset StartTime { get => TimeRange.Start; }
        public DateTimeOffset EndTime { get => TimeRange.End; }
       
        internal void OverlapGuard(Booking otherBooking)
        {
            if ((this.StartTime < otherBooking.EndTime) && (this.EndTime > otherBooking.StartTime)) throw new Exception("You cannot make overlapping bookings");
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.BookingAdded e:
                    Id = new BookingId(e.BookingId);
                    BookedBy = new BookingBookedBy(e.BookedBy);
                    TimeRange = new TimeRange(e.Start, e.End);
                    break;
            }

        }
        public Booking(Action<object> applier) : base(applier)
        {
        }
    }
}
