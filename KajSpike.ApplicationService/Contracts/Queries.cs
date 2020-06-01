using KajSpike.ApplicationService.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static KajSpike.ApplicationService.Projections.ReadModels;
using static KajSpike.ApplicationService.QueryModels;

namespace KajSpike.ApplicationService.Contracts
{
    public static class Queries
    {
        public static CalendarDetails Query(
            this IEnumerable<ReadModels.CalendarDetails> items,
            GetCalendar query
            )
        { 
            return items.FirstOrDefault(x => x.CalendarId == query.CalendarId); 
        }
    }
}
