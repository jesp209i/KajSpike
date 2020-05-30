using KajSpike.Framework;

namespace KajSpike.Domain.ValueObjects
{
    public class BookingBookedBy: ValueObject<BookingBookedBy>
    {
        private readonly string _value;
        internal BookingBookedBy(string name) => _value = name;

        public static BookingBookedBy FromString(string bookerName)
        {
            BookedByGuard(bookerName);
            return new BookingBookedBy(bookerName);
        }
        public static implicit operator string(BookingBookedBy self) => self._value;
        private static void BookedByGuard(string name)
        {
            if (name.Length < 10)
                throw new BadCalendarDescriptionException("Name of booker must be longer than 10 characters.");
        }
    }
}
