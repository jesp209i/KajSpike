using KajSpike.ApplicationService.Contracts;
using KajSpike.Domain;
using KajSpike.Domain.ValueObjects;
using KajSpike.Framework.Interfaces;
using KajSpike.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using static KajSpike.ApplicationService.Contracts.BookingCalendars;

namespace KajSpike.ApplicationService
{
    public class CalendarApplicationService : IApplicationService
    {
        private readonly ICalendarRepository _repository;

        public CalendarApplicationService(ICalendarRepository repository)
        {
            _repository = repository;
        }
        public Task Handle(object command) =>
            command switch
            {
                V1.CreateCalendar cmd =>
                    HandleCreate(cmd),
                V1.UpdateCalendarDescription cmd =>
                    HandleUpdate(
                        cmd.CalendarId,
                        c => c.ChangeDescription(CalendarDescription.FromString(cmd.NewDescription))
                        ),
                V1.UpdateCalendarMaxBookingTimeInMinutes cmd =>
                    HandleUpdate(
                        cmd.CalendarId,
                        c => c.ChangeMaximumBookingTimeInMinutes(CalendarMaximumBookingTimeInMinutes.FromNumber(cmd.NewMaxBookingTimeInMinutes))
                        ),
                V1.AddBooking cmd =>
                    HandleUpdate(
                        cmd.CalendarId,
                        c => c.AddBooking(BookingBookedBy.FromString(cmd.NameOfBooker), TimeRange.MakeTimeRange(cmd.Start, cmd.End,c.MaximumBookingTimeInMinutes))),
                V1.RemoveBooking cmd =>
                    HandleUpdate(cmd.CalendarId,
                        c => c.RemoveBooking(BookingId.FromGuid(cmd.BookingId))),
                _ => Task.CompletedTask
            };

        private async Task HandleCreate(V1.CreateCalendar cmd)
        {
            if (await _repository.Exists(CalendarId.FromGuid(cmd.CalendarId)))
                throw new InvalidOperationException(
                    $"Entity with id {cmd.CalendarId} already exists");

            var calendar = new Calendar(
                CalendarId.FromGuid(cmd.CalendarId),
                CalendarDescription.FromString(cmd.Description),
                CalendarMaximumBookingTimeInMinutes.FromNumber(cmd.MaxBookingTimeInMinutes)
            );

            await _repository.Add(calendar);
        }

        private async Task HandleUpdate(Guid calendarId, Action<Calendar> operation)
        {
            var calendar = await _repository.Load(CalendarId.FromGuid(calendarId));
            if (calendar == null)
                throw new InvalidOperationException(
                    $"Entity with id {calendarId} cannot be found"
                );

            operation(calendar);

            await _repository.Add(calendar);
        }
    }
}
