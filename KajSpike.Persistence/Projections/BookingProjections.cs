using KajSpike.Domain;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KajSpike.Persistence.Projections
{
    public class BookingProjections : RavenDbProjection<ReadModels.BookingDetails>
    {
        public BookingProjections(Func<IAsyncDocumentSession> getSession): base(getSession) {}
        public override Task Project(object @event)
        =>
            @event switch
            {
                Events.CalendarCreated e => Task.CompletedTask,
                Events.CalendarDescriptionChanged e => Task.CompletedTask,
                Events.CalendarMaxBookingTimeChanged e => Task.CompletedTask,
                Events.BookingAdded e=>
                    Create(()=> Task.FromResult(new ReadModels.BookingDetails
                    { 
                        CalendarId = e.CalendarId,
                        BookingId = e.BookingId,
                        BookedBy = e.BookedBy,
                        StartTime = e.Start,
                        EndTime = e.End
                    })),
                Events.BookingRemoved e =>
                    Remove(b => b.BookingId == e.BookingId),
                _ => Task.CompletedTask
            };
    }
}
