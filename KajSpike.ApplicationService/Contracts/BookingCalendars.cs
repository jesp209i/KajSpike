using System;

namespace KajSpike.ApplicationService.Contracts
{
    public static class BookingCalendars
    {
        public static class V1
        {
            public class CreateCalendar
            {
                public Guid CalendarId { get; set; }
                public string Description { get; set; }
                public int MaxBookingTimeInMinutes { get; set; }
            }
            public class UpdateCalendarDescription
            {
                public Guid CalendarId { get; set; }
                public string NewDescription { get; set; }
            }
            public class UpdateCalendarMaxBookingTimeInMinutes
            {
                public Guid CalendarId { get; set; }
                public int NewMaxBookingTimeInMinutes { get; set; }
            }
            public class AddBooking
            {
                public Guid CalendarId { get; set; }
                public string NameOfBooker { get; set; }
                public DateTimeOffset Start { get; set; }
                public DateTimeOffset End { get; set; }
            }
            public class RemoveBooking
            {
                public Guid CalendarId { get; set; }
                public Guid BookingId { get; set; }
            }
        }
    }
}
