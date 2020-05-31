using KajSpike.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace KajSpike.ApplicationService
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<CalendarApplicationService>();
            return services;
        }
    }
}
