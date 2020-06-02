using KajSpike.Infrastructure.Projections;
using System.Collections.Generic;
using System.Linq;
using static KajSpike.ApplicationService.QueryModels;
using static KajSpike.Infrastructure.Projections.ReadModels;

namespace KajSpike.ApplicationService.Contracts
{
    public static class Queries
    {
        public static IEnumerable<CalendarOverview> Query(
            this IEnumerable<ReadModels.CalendarOverview> allCalendars,
            GetCalendars query
            )
        {
            return allCalendars;
        }
        public static CalendarDetails Query(
            this IEnumerable<ReadModels.CalendarDetails> allCalendars,
            GetCalendar query
            )
        {
            return allCalendars.FirstOrDefault(x => x.CalendarId == query.CalendarId);
        }
        public static IEnumerable<BookingDetails> Query(
            this IEnumerable<ReadModels.BookingDetails> allBookings,
            GetBookingsInCaledar query
            )
        {
            return allBookings.Where(x => x.CalendarId == query.CalendarId).ToList();
        }
    }
}
