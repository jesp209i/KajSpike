using KajSpike.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace KajSpike.Domain
{
    public class Booking
    {
        private readonly TimeRange _timeRange;
        public BookingId BookingId { get; set; }
        public BookingBookedBy BookedBy { get; set; }
        public DateTimeOffset StartTime { get => _timeRange.Start; }
        public DateTimeOffset EndTime { get => _timeRange.End; }
        public Booking(string name, DateTimeOffset start, DateTimeOffset end, int maximumBookingTimeInMinutes)
        {
            BookingId = BookingId.FromGuid(Guid.NewGuid());
            BookedBy = BookingBookedBy.FromString(name);
            _timeRange = TimeRange.MakeTimeRange(start, end, maximumBookingTimeInMinutes);
        }

        internal void OverlapGuard(Booking otherBooking)
        {
            if ((this.StartTime < otherBooking.EndTime) && (this.EndTime > otherBooking.StartTime)) throw new Exception("You cannot make overlapping bookings");
        }
    }
}
