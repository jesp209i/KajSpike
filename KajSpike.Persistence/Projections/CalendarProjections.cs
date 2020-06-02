using KajSpike.Domain;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KajSpike.Persistence.Projections
{
    public class CalendarProjections : RavenDbProjection<ReadModels.CalendarDetails>
    {
        public CalendarProjections(Func<IAsyncDocumentSession> getSession): base(getSession) {}
        public override Task Project(object @event)
        =>
            @event switch
            {
                Events.CalendarCreated e =>
                    Create(() => Task.FromResult(new ReadModels.CalendarDetails
                    {
                        CalendarId = e.CalendarId,
                        Description = e.CalendarDescription,
                        MaximumBookingTimeInMinutes = e.MaximumBookingTimeInMinutes,
                        NumberOfBookings = 0
                    })),
                Events.CalendarDescriptionChanged e =>
                    UpdateOne(
                        e.CalendarId,
                        c => c.Description = e.NewCalendarDescription
                    ),
                Events.CalendarMaxBookingTimeChanged e =>
                    UpdateOne(
                        e.CalendarId,
                        c => c.MaximumBookingTimeInMinutes = e.NewMaximumBookingTimeInMinutes
                    ),
                Events.BookingAdded e=>
                    UpdateOne(e.CalendarId,
                        c => c.NumberOfBookings++),
                Events.BookingRemoved e =>
                    UpdateOne(e.CalendarId,
                        c => c.NumberOfBookings--),
                _ => Task.CompletedTask
            };

    }
}
