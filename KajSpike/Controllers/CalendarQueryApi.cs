using System.Threading.Tasks;
using KajSpike.Persistence.Projections;
using KajSpike.Framework.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using KajSpike.Persistence;
using static KajSpike.Persistence.Queries;
using Raven.Client.Documents.Session;

namespace KajSpike.Controllers
{
    [ApiController]
    [Route("calendar")]
    public class CalendarQueryApi : Controller
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IRequestHandler _handler;

        public CalendarQueryApi(IAsyncDocumentSession session, IRequestHandler handler)
        {
            _session = session;
            _handler = handler;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
            => await _handler.HandleQuery(async () => await _session.Query(new QueryModels.GetCalendars()));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
            => await _handler.HandleQuery(async () => await _session.Query(new QueryModels.GetCalendar { CalendarId = Guid.Parse(id) }));

        [HttpGet("{id}/bookings")]
        public async Task<IActionResult> GetBookings(string id)
            => await _handler.HandleQuery(async () => await _session.Query(new QueryModels.GetBookingsInCaledar { CalendarId = Guid.Parse(id) }));
    }
}
