using KajSpike.Domain.ValueObjects;
using KajSpike.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KajSpike.Domain
{
    public class Calendar : AggregateRoot<CalendarId>
    {
        public CalendarDescription Description { get; private set; }
        public CalendarMaximumBookingTimeInMinutes MaximumBookingTimeInMinutes { get; private set; }
        public List<Booking> Bookings { get; private set; }
        public Calendar(CalendarId id, CalendarDescription description, CalendarMaximumBookingTimeInMinutes maxBookingTimeInMinutes)
        {
            Bookings = new List<Booking>();
            Apply(new Events.CalendarCreated
            {
                CalendarId = id,
                CalendarDescription = description,
                MaximumBookingTimeInMinutes = maxBookingTimeInMinutes
            });
        }
        public void ChangeDescription(CalendarDescription newDescription)
        {
            Apply(new Events.CalendarDescriptionChanged {
                CalendarId = this.Id,
                NewCalendarDescription = newDescription
            });
        }
        public void ChangeMaximumBookingTimeInMinutes(CalendarMaximumBookingTimeInMinutes newMaximumBookingTimeInMinutes)
        {
            Apply(new Events.CalendarMaxBookingTimeChanged
            {
                CalendarId = this.Id,
                NewMaximumBookingTimeInMinutes = newMaximumBookingTimeInMinutes
            });
        }
        public void AddBooking(BookingBookedBy nameOfBooker, TimeRange timeRange)
        {
            Apply(new Events.BookingAdded { 
                CalendarId = this.Id,
                BookingId = Guid.NewGuid(),
                BookedBy = nameOfBooker,
                Start = timeRange.Start,
                End = timeRange.End,
            });
        }
        public void RemoveBooking(BookingId bookingToRemove)
        {
            Apply(new Events.BookingRemoved
            {
                CalendarId = this.Id,
                BookingId = new BookingId(bookingToRemove)
            });
        }

        private void EnsureNoBookingConflicts(Booking newBooking) => 
            Bookings.ForEach(x => newBooking.OverlapGuard(x));
        
        protected override void When(object @event)
        {
            switch(@event)
            {
                case Events.CalendarCreated e:
                    Id = new CalendarId(e.CalendarId);
                    Description = new CalendarDescription(e.CalendarDescription);
                    MaximumBookingTimeInMinutes = new CalendarMaximumBookingTimeInMinutes(e.MaximumBookingTimeInMinutes);
                    break;
                case Events.CalendarDescriptionChanged e:
                    Description = new CalendarDescription(e.NewCalendarDescription);
                    break;
                case Events.CalendarMaxBookingTimeChanged e:
                    MaximumBookingTimeInMinutes = new CalendarMaximumBookingTimeInMinutes(e.NewMaximumBookingTimeInMinutes);
                    break;
                case Events.BookingAdded e:
                    var newBooking = new Booking(Apply);
                    ApplyToEntity(newBooking, e);
                    EnsureNoBookingConflicts(newBooking);
                    Bookings.Add(newBooking);
                    break;
                case Events.BookingRemoved e:
                    var booking = Bookings.Where(booking => booking.Id == e.BookingId).FirstOrDefault();
                    if (booking == null) throw new Exception("Booking doesnot exist");
                    Bookings.Remove(booking);
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            bool validDescription = (Description != default);
            bool validMaxBookingTimeInMinutes = (MaximumBookingTimeInMinutes > 0);
            if (validDescription == validMaxBookingTimeInMinutes == false)
                throw new Exception("Calendar not valid");
        }
        protected Calendar() { Bookings = new List<Booking>(); }
    }
}
