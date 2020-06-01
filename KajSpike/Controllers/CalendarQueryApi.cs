using System.Threading.Tasks;
using KajSpike.ApplicationService;
using Microsoft.AspNetCore.Mvc;


namespace KajSpike.Controllers
{
    [ApiController]
    [Route("calendar")]
    public class CalendarQueryApi : Controller
    {
        private readonly CalendarApplicationService _appService;
        public CalendarQueryApi(CalendarApplicationService appService)
        {
            _appService = appService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(ApplicationService.QueryModels.GetCalendars query)
        {
            await _appService.Handle(query);
            return Ok();
        }
        [Route("Bookings")]
        [HttpGet]
        public async Task<IActionResult> Get(ApplicationService.QueryModels.GetBookingsInCalendar query)
        {
            await _appService.Handle(query);
            return Ok();
        }
    }
}
