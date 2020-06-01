using KajSpike.ApplicationService.Projections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KajSpike.ApplicationService
{
    public static class QueryModels
    {
        public class GetCalendars {
            public int Page { get; set; }
            public int PageSize { get; set; }
        }
        public class GetCalendar
        {
            public Guid CalendarId { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
        }
        public class GetBookingsInCaledar
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public Guid CalendarId { get; set; }
        }
    }
}
