using System.Collections.Generic;
using System.Threading.Tasks;
using KajSpike.ApplicationService;
using KajSpike.ApplicationService.Contracts;
using KajSpike.Persistence.Projections;
using KajSpike.Framework.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace KajSpike.Controllers
{
    [ApiController]
    [Route("calendar")]
    public class CalendarQueryApi : Controller
    {
        private readonly IEnumerable<ReadModels.CalendarDetails> _calendars;
        private readonly IEnumerable<ReadModels.BookingDetails> _bookings;
        private readonly IRequestHandler _handler;

        public CalendarQueryApi(IEnumerable<ReadModels.CalendarDetails> items, IEnumerable<ReadModels.BookingDetails> bookings, IRequestHandler handler)
        {
            _calendars = items;
            _bookings = bookings;
            _handler = handler;
        }
        [HttpGet]
        public Task<IActionResult> Get()
            => _handler.HandleQuery(() => _calendars.Query(new QueryModels.GetCalendars()));

        [HttpGet("{id}")]
        public Task<IActionResult> Get(string id)
            => _handler.HandleQuery(() => _calendars.Query(new QueryModels.GetCalendar { CalendarId = Guid.Parse(id) }));

        [HttpGet("{id}/bookings")]
        public Task<IActionResult> GetBookings(string id)
            => _handler.HandleQuery(() => _bookings.Query(new QueryModels.GetBookingsInCaledar { CalendarId = Guid.Parse(id) }));
    }
}
