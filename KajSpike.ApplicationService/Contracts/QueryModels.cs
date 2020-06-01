using System;

namespace KajSpike.ApplicationService
{
    public static class QueryModels
    {
        public class GetCalendars {
        }
        public class GetCalendar
        {
            public Guid CalendarId { get; set; }
        }
        public class GetBookingsInCaledar
        {
            public Guid CalendarId { get; set; }
        }
    }
}
