using System;
using System.Collections.Generic;
using System.Text;

namespace KajSpike.ApplicationService
{
    public static class QueryModels
    {
        public class GetCalendars
        {
        }
        public class GetBookingsInCalendar
        {
            public Guid CalendarId { get; set; }
        }

    }
}
