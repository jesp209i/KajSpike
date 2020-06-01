using KajSpike.Framework;
using System;

namespace KajSpike.Domain.ValueObjects
{
    public class CalendarId: ValueObject<CalendarId>
    {
        private readonly Guid _value;
        public CalendarId(Guid value) => _value = value;
        public static CalendarId FromGuid(Guid id)
        {
            CalendarIdGuard(id);
            return new CalendarId(id);
        }
        public static CalendarId FromString(string id)
        {
            Guid newId;
            if (!Guid.TryParse(id, out newId)) throw new Exception("Bad stringformat for CalendarId");
            return FromGuid(newId);
        }
        public static implicit operator Guid(CalendarId self) => self._value;
        public override string ToString() => _value.ToString();
        private static void CalendarIdGuard(Guid id)
        {
            if (id == default) throw new ArgumentNullException(nameof(id), "CalendarId must be specified");
        }
    }
}
