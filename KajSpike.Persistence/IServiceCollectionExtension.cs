using KajSpike.Domain.Interfaces;
using KajSpike.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KajSpike.Persistence
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddPersistenceConfiguration(this IServiceCollection services)
        {
            const string connectionString = "Server=db;Database=master;User=sa;Password=Horse123!";
            services.AddDbContext<CalendarDbContext>(options =>
                //options.UseInMemoryDatabase("CalendarBooking"));
                options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<ICalendarRepository, CalendarRepository>();
            return services;
        }
    }
}
