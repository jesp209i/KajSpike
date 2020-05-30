using KajSpike.Framework;
using System;

namespace KajSpike.Domain.ValueObjects
{
    public class CalendarMaximumBookingTimeInMinutes: ValueObject<CalendarMaximumBookingTimeInMinutes>
    {
        private readonly int _value;
        internal CalendarMaximumBookingTimeInMinutes(int value) => _value = value;

        public static CalendarMaximumBookingTimeInMinutes FromNumber(int number) 
        {
            MaximumBookingTimeInMunutesGuard(number);
            return new CalendarMaximumBookingTimeInMinutes(number); 
        }
        public static CalendarMaximumBookingTimeInMinutes FromString(string number)
        {
            if (!int.TryParse(number, out int newNumber)) throw new Exception("String is not a valid value for Maximum booking time in minutes");
            return FromNumber(newNumber);
        }
        public static implicit operator int(CalendarMaximumBookingTimeInMinutes self) => self._value;
        public override string ToString() => _value.ToString();
        private static void MaximumBookingTimeInMunutesGuard(int value)
        {
            if (value < 0) throw new Exception("Maximum booking time in minutes must be larger than 0");
        }
    }
}
