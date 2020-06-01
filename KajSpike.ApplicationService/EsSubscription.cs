using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using KajSpike.ApplicationService.Projections;
using KajSpike.Domain;
using KajSpike.Persistence;
using Serilog.Events;
using ILogger = Serilog.ILogger;


namespace KajSpike.ApplicationService
{
    public class EsSubscription
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<EsSubscription>();

        private readonly IEventStoreConnection _connection;
        private readonly IList<ReadModels.CalendarDetails> _items;
        private EventStoreAllCatchUpSubscription _subscription;

        public EsSubscription(IEventStoreConnection connection, IList<ReadModels.CalendarDetails> items)
        {
            _connection = connection;
            _items = items;
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
                    _items.Add(new ReadModels.CalendarDetails
                    {
                        CalendarId = e.CalendarId,
                        Description = e.CalendarDescription,
                        MaximumBookingTimeInMinutes = e.MaximumBookingTimeInMinutes

                    });
                    break;
                case Events.CalendarDescriptionChanged e:
                    UpdateItem(e.CalendarId, c => c.Description = e.NewCalendarDescription);
                    break;
                case Events.CalendarMaxBookingTimeChanged e:
                    UpdateItem(e.CalendarId, c => c.MaximumBookingTimeInMinutes = e.NewMaximumBookingTimeInMinutes);
                    break;
                case Events.BookingAdded e:
                    UpdateItem(e.Id, ad =>
                    {
                        ad.Price = e.Price;
                        ad.CurrencyCode = e.CurrencyCode;
                    });
                    break;
            }

            return Task.CompletedTask;
        }

        private void UpdateItem(Guid id, Action<ReadModels.CalendarDetails> update)
        {
            var item = _items.FirstOrDefault(x => x.CalendarId == id);
            if (item == null) return;

            update(item);
        }

        public void Stop() => _subscription.Stop();
    }
}