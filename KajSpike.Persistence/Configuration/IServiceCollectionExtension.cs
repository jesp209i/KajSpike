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
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace KajSpike.Persistence.Configuration
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddPersistenceConfiguration(this IServiceCollection services)
        {
            const string connectionString = "ConnectTo=tcp://admin:changeit@localhost:1113;DefaultUserCredentials=admin:changeit;";
            var esConnection = EventStoreConnection.Create(connectionString, ConnectionSettings.Create().KeepReconnecting(), "KajSpike");
            var store = new EsAggregateStore(esConnection);

            var documentStore = ConfigureRavenDb("http://localhost:8080", "KajSpike_readmodels");
            
            Func<IAsyncDocumentSession> getSession = () => documentStore.OpenAsyncSession();
            services.AddTransient(c => getSession());

            services.AddSingleton(esConnection);
            services.AddSingleton<IAggregateStore>(store);

            var projectionManager = new ProjectionManager(
                esConnection,
                new RavenDbCheckpointStore(getSession, "readmodels"),
                new CalendarProjections(getSession),
                new BookingProjections(getSession));

            services.AddSingleton<IHostedService>(new EventStoreService(esConnection, projectionManager));

            return services;
        }
        private static IDocumentStore ConfigureRavenDb(string url, string database)
        {
            var store = new DocumentStore
            {
                Urls = new[] { url },
                Database = database
            };
            store.Initialize();
            var record = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));
            if (record == null)
            {
                store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(store.Database)));
            }
            return store;
        }
    }
}
