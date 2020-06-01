using System.Collections.Generic;
using System.Threading.Tasks;
using KajSpike.ApplicationService;
using KajSpike.ApplicationService.Contracts;
using KajSpike.ApplicationService.Projections;
using KajSpike.Framework.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace KajSpike.Controllers
{
    [ApiController]
    [Route("calendar")]
    public class CalendarQueryApi : Controller
    {
        private readonly IEnumerable<ReadModels.CalendarDetails> _items;
        private readonly IRequestHandler _handler;

        public CalendarQueryApi(IEnumerable<ReadModels.CalendarDetails> items, IRequestHandler handler)
        {
            _items = items;
            _handler = handler;
        }
        [HttpGet]
        public Task<IActionResult> Get(QueryModels.GetCalendar request) 
            => _handler.HandleQuery(() => _items.Query(request));
    }
}
