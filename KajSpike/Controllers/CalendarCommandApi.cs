using System.Threading.Tasks;
using KajSpike.ApplicationService;
using Microsoft.AspNetCore.Mvc;


namespace KajSpike.Controllers
{
    [ApiController]
    [Route("calendar")]
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
        [Route("description")]
        [HttpPut]
        public async Task<IActionResult> Put(ApplicationService.Contracts.BookingCalendars.V1.UpdateCalendarDescription command)
        {
            _appService.Handle(command);
            return Ok();
        }
        [Route("MaxBookingTime")]
        [HttpPut]
        public async Task<IActionResult> Put(ApplicationService.Contracts.BookingCalendars.V1.UpdateCalendarMaxBookingTimeInMinutes command)
        {
            _appService.Handle(command);
            return Ok();
        }
        [Route("Booking")]
        [HttpPost]
        public async Task<IActionResult> Post(ApplicationService.Contracts.BookingCalendars.V1.AddBooking command)
        {
            _appService.Handle(command);
            return Ok();
        }
        [Route("Booking")]
        [HttpDelete]
        public async Task<IActionResult> Delete(ApplicationService.Contracts.BookingCalendars.V1.RemoveBooking command)
        {
            _appService.Handle(command);
            return Ok();
        }
    }
}
