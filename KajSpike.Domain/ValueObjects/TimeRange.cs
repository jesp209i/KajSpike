using KajSpike.Framework;
using System;

namespace KajSpike.Domain.ValueObjects
{
    public class TimeRange : ValueObject<TimeRange>
    {
        public DateTimeOffset Start { get; private set; }
        public DateTimeOffset End { get; private set; }
        internal TimeRange(DateTimeOffset start, DateTimeOffset end)
        {
            Start = start;
            End = end;
        }
        public static TimeRange MakeTimeRange(DateTimeOffset start, DateTimeOffset end, int maximumMinutes)
        {
            TimeRangeGuard(start, end, maximumMinutes);
            return new TimeRange(start, end);
        }
        private static void TimeRangeGuard(DateTimeOffset start, DateTimeOffset end, int maximumMinutesInTimeRange)
        {
            if (start > end) throw new Exception("Start time cannot be at at later time than End time");
            if (end.Subtract(start) > TimeSpan.FromMinutes(maximumMinutesInTimeRange)) 
                throw new Exception($"Booking cannot be exceed {maximumMinutesInTimeRange} minutes");
        }
    }
}
