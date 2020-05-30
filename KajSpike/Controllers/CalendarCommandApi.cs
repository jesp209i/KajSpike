using System.Threading.Tasks;
using KajSpike.ApplicationService;
using Microsoft.AspNetCore.Mvc;


namespace KajSpike.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarCommandApi : Controller
    {
        private readonly CalendarApplicationService _appService;
        public CalendarCommandApi(CalendarApplicationService appService)
        {
            _appService = appService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(ApplicationService.Contracts.BookingCalendars.V1.CreateCalendar command)
        {
            _appService.Handle(command);
            return Ok();

        }
    }
}
