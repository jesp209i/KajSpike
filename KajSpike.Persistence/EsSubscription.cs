﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using KajSpike.Domain;
using KajSpike.Persistence.Projections;
using Serilog.Events;
using ILogger = Serilog.ILogger;


namespace KajSpike.Persistence
{
    public class EsSubscription
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<EsSubscription>();

        private readonly IEventStoreConnection _connection;
        private readonly IList<ReadModels.CalendarDetails> _calendarDetails; 
        private readonly IList<ReadModels.BookingDetails> _bookingDetails;
        private EventStoreAllCatchUpSubscription _subscription;

        public EsSubscription(IEventStoreConnection connection, IList<ReadModels.CalendarDetails> calendarDetails, IList<ReadModels.BookingDetails> bookingDetails)
        {
            _connection = connection;
            _calendarDetails = calendarDetails;
            _bookingDetails = bookingDetails;
        }

        public void Start()
        {
            var settings = new CatchUpSubscriptionSettings(2000, 500,
                Log.IsEnabled(LogEventLevel.Verbose),
                true, "try-out-subscription");

            _subscription = _connection.SubscribeToAllFrom(Position.Start, settings, EventAppeared);
        }

        private Task EventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$")) return Task.CompletedTask;

            var @event = resolvedEvent.Deserialize();

            Log.Debug("Projecting event {type}", @event.GetType().Name);

            switch (@event)
            {
                case Events.CalendarCreated e:
                    _calendarDetails.Add(new ReadModels.CalendarDetails
                    {
                        CalendarId = e.CalendarId,
                        Description = e.CalendarDescription,
                        MaximumBookingTimeInMinutes = e.MaximumBookingTimeInMinutes,
                        NumberOfBookings = 0
                    });
                    break;
                case Events.CalendarDescriptionChanged e:
                    UpdateItem(e.CalendarId, c => c.Description = e.NewCalendarDescription);
                    break;
                case Events.CalendarMaxBookingTimeChanged e:
                    UpdateItem(e.CalendarId, c => c.MaximumBookingTimeInMinutes = e.NewMaximumBookingTimeInMinutes);
                    break;
                case Events.BookingAdded e:
                    UpdateItem(e.CalendarId, c => c.NumberOfBookings++);
                    _bookingDetails.Add(new ReadModels.BookingDetails 
                    { 
                        CalendarId = e.CalendarId,
                        BookingId = e.BookingId,
                        BookedBy = e.BookedBy,
                        StartTime = e.Start,
                        EndTime = e.End
                    });
                    break;
                case Events.BookingRemoved e:
                    UpdateItem(e.CalendarId, c => c.NumberOfBookings--);
                    _bookingDetails.Remove(
                        _bookingDetails.FirstOrDefault(b => b.BookingId == e.BookingId));
                    break;
            }

            return Task.CompletedTask;
        }

        private void UpdateItem(Guid id, Action<ReadModels.CalendarDetails> update)
        {
            var item = _calendarDetails.FirstOrDefault(x => x.CalendarId == id);
            if (item == null) return;

            update(item);
        }

        public void Stop() => _subscription.Stop();
    }
}