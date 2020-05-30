using KajSpike.ApplicationService.Contracts;
using KajSpike.Framework.Interfaces;
using System;
using System.Threading.Tasks;

namespace KajSpike.ApplicationService
{
    public class CalendarApplicationService : IHandleCommand<Contracts.BookingCalendars.V1.CreateCalendar>
    {
        public Task Handle(BookingCalendars.V1.CreateCalendar command)
        {
            throw new NotImplementedException();
        }
    }
}
