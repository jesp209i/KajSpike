using System;
using System.Collections.Generic;
using System.Text;

namespace KajSpike.Infrastructure.Projections
{
    public static class ReadModels
    {
        public class CalendarDetails
        {
            public Guid CalendarId { get; set; }
            public string Description { get; set; }
            public int MaximumBookingTimeInMinutes { get; set; }
            public int NumberOfBookings { get; set; }
        }
        public class BookingDetails
        {
            public Guid CalendarId { get; set; }
            public Guid BookingId { get; set; }
            public string BookedBy { get; set; }
            public DateTimeOffset StartTime { get; set; }
            public DateTimeOffset EndTime { get; set; }
        }
    }
}
