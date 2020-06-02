using KajSpike.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using KajSpike.Infrastructure.Projections;

namespace KajSpike.Infrastructure
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddPersistenceConfiguration(this IServiceCollection services)
        {
            const string connectionString = "ConnectTo=tcp://admin:changeit@localhost:1113;DefaultUserCredentials=admin:changeit;";
            var esConnection = EventStoreConnection.Create(connectionString, ConnectionSettings.Create().KeepReconnecting(), "KajSpike");
            var store = new EsAggregateStore(esConnection);

            var calenderDetails = new List<ReadModels.CalendarDetails>();
            var calenderOverviews = new List<ReadModels.CalendarOverview>();
            var bookingDetails = new List<ReadModels.BookingDetails>();
            services.AddSingleton<IEnumerable<ReadModels.CalendarDetails>>(calenderDetails);
            services.AddSingleton<IEnumerable<ReadModels.CalendarOverview>>(calenderOverviews);
            services.AddSingleton<IEnumerable<ReadModels.BookingDetails>>(bookingDetails);

            var subscription = new EsSubscription(esConnection, calenderDetails, calenderOverviews, bookingDetails);
            
            services.AddSingleton(esConnection);
            services.AddSingleton<IAggregateStore>(store);
            services.AddSingleton<IHostedService>(new EventStoreService(esConnection,subscription));
            return services;
        }
    }
}
