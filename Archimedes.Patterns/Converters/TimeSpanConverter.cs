using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Converters
{
    public static class TimeSpanConverter
    {
        public static DateTime Convert(TimeSpan value) {
            TimeSpan span = TimeSpan.FromTicks((value).Ticks);
            return DateTime.MinValue.Add(span);
        }

        public static TimeSpan ConvertBack(DateTime value) {
            DateTime datetime = (DateTime)value;
            return TimeSpan.FromTicks(Math.Abs((DateTime.MinValue - datetime).Ticks));
        }
    }

    public static class TimeSpanFixedMonthConverter
    {
        public static DateTime Convert(TimeSpan value) {

            int fullMonts = value.Days / 30;
            value = value - (TimeSpan.FromDays(30 * fullMonts));

            TimeSpan span = TimeSpan.FromTicks((value).Ticks);


            return DateTime.MinValue.Add(span);
        }

        public static TimeSpan ConvertBack(DateTime value) {
            DateTime datetime = (DateTime)value;
            return TimeSpan.FromTicks(Math.Abs((DateTime.MinValue - datetime).Ticks));
        }
    }
}
