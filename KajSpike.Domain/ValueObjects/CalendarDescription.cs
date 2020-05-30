using KajSpike.Framework;
using System;

namespace KajSpike.Domain.ValueObjects
{
    public class CalendarDescription: ValueObject<CalendarDescription>
    {
        private readonly string _value;
        internal CalendarDescription(string value) => _value = value;
        public static CalendarDescription FromString(string description)
        {
            CalendarDescriptionGuard(description);
            return new CalendarDescription(description);
        }
        public static implicit operator string(CalendarDescription self) => self._value;
        private static void CalendarDescriptionGuard(string description)
        {
            int minimumDescriptionLength = 10;
            if (description.Length < minimumDescriptionLength)
                throw new BadCalendarDescriptionException($"Description must be at least {minimumDescriptionLength} characters long.");
        }
    }
    public class BadCalendarDescriptionException : Exception
    {
        public BadCalendarDescriptionException(string message) : base(message)
        {
        }
    }
}
