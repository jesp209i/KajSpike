using KajSpike.Domain.ValueObjects;
using KajSpike.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KajSpike.Domain
{
    public class Calendar : Entity
    {
        public CalendarId Id { get; set; }
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
        public void AddBooking(string nameOfBooker, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var booking = new Booking(nameOfBooker, startTime, endTime, MaximumBookingTimeInMinutes);
            EnsureNoBookingConflicts(booking);
            Bookings.Add(booking);
        }
        public void RemoveBooking(BookingId bookingToRemove)
        {
            var booking = Bookings.Where(booking => booking.BookingId == bookingToRemove).FirstOrDefault();
            if (booking == null) throw new Exception("Booking doesnot exist");
            Bookings.Remove(booking);
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
                    throw new NotImplementedException("When Booking added switch case");
                    break;
                case Events.BookingRemoved e:
                    throw new NotImplementedException("When Booking removed switch case");
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
    }
}
