using KajSpike.Domain.Interfaces;
using KajSpike.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EventStore.ClientAPI;
using System;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using KajSpike.Persistence.Projections;

namespace KajSpike.Persistence
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddPersistenceConfiguration(this IServiceCollection services)
        {
            const string connectionString = "ConnectTo=tcp://admin:changeit@localhost:1113;DefaultUserCredentials=admin:changeit;";
            var esConnection = EventStoreConnection.Create(connectionString, ConnectionSettings.Create().KeepReconnecting(), "KajSpike");
            var store = new EsAggregateStore(esConnection);

            var calenderDetails = new List<ReadModels.CalendarDetails>();
            var bookingDetails = new List<ReadModels.BookingDetails>();
            services.AddSingleton<IEnumerable<ReadModels.CalendarDetails>>(calenderDetails);
            services.AddSingleton<IEnumerable<ReadModels.BookingDetails>>(bookingDetails);

            var subscription = new EsSubscription(esConnection, calenderDetails, bookingDetails);
            
            services.AddSingleton(esConnection);
            services.AddSingleton<IAggregateStore>(store);
            services.AddSingleton<IHostedService>(new EventStoreService(esConnection,subscription));
            return services;
        }
    }
}
