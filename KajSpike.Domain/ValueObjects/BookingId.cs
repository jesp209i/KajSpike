using KajSpike.Framework;
using System;

namespace KajSpike.Domain.ValueObjects
{
    public class BookingId : ValueObject<BookingId>
    {
        private readonly Guid _value;
        internal BookingId(Guid value) => _value = value;
        public static BookingId FromGuid(Guid id)
        {
            BookingIdGuard(id);
            return new BookingId(id); 
        }
        public static implicit operator Guid(BookingId self) => self._value;
        public override string ToString() => _value.ToString();
        private static void BookingIdGuard(Guid value)
        {
            if (value == default) throw new ArgumentNullException(nameof(value), "BookingId must be specified");
        }
    }
}
