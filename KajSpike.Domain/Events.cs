using System;
using System.Collections.Generic;
using System.Text;

namespace KajSpike.Domain
{
    public static class Events
    {
        public class CalendarCreated
        {
            public Guid CalendarId { get; set; }
            public string CalendarDescription { get; set; }
            public int MaximumBookingTimeInMinutes { get; set; }
        }
        public class CalendarDescriptionChanged
        {
            public Guid CalendarId { get; set; }
            public string NewCalendarDescription { get; set; }
        }
        public class CalendarMaxBookingTimeChanged
        {
            public Guid CalendarId { get; set; }
            public int NewMaximumBookingTimeInMinutes { get; set; }
        }
        public class BookingAdded
        {
            public Guid CalendarId { get; set; }
            public Guid BookingId { get; set; }
            public string BookedBy { get; set; }
            public DateTimeOffset Start { get; set; }
            public DateTimeOffset End { get; set; }
        }
        public class BookingRemoved
        {
            public Guid CalendarId { get; set; }
            public Guid BookingId { get; set; }
        }
    }
}
