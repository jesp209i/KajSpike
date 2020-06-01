using KajSpike.Domain.Interfaces;
using KajSpike.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EventStore.ClientAPI;
using System;
using Microsoft.Extensions.Hosting;

namespace KajSpike.Persistence
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddPersistenceConfiguration(this IServiceCollection services)
        {
            const string connectionString = "ConnectTo=tcp://admin:changeit@localhost:1113;DefaultUserCredentials=admin:changeit;";
            var esConnection = EventStoreConnection.Create(connectionString, ConnectionSettings.Create().KeepReconnecting(), "KajSpike");
            var store = new EsAggregateStore(esConnection);
            services.AddSingleton(esConnection);
            services.AddSingleton<IAggregateStore>(store);
            services.AddSingleton<IHostedService, HostedService>();
            return services;
        }
    }
}
