using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static KajSpike.Persistence.Projections.ReadModels;
using static KajSpike.Persistence.QueryModels;

namespace KajSpike.Persistence
{
    public static class Queries
    {
        public async static Task<IEnumerable<CalendarDetails>> Query(
            this IAsyncDocumentSession session,
            GetCalendars query
            )
        {
            return await session.LoadAsync<IEnumerable<CalendarDetails>>(new Guid().ToString());
        }
        public async static Task<CalendarDetails> Query(
            this IAsyncDocumentSession session,
            GetCalendar query
            )
        {
            return await session.LoadAsync<CalendarDetails>(query.CalendarId.ToString());
        }
        public async static Task<IEnumerable<BookingDetails>> Query(
            this IAsyncDocumentSession session,
            GetBookingsInCaledar query
            )
        {
            return await session.LoadAsync<IEnumerable<BookingDetails>>(query.CalendarId.ToString());
        }
    }
}
