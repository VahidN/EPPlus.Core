namespace System
{
    /// <summary>
    /// Missing DateTime Parts
    /// </summary>
    public static class DateTimeExtensions
    {
        // Number of 100ns ticks per time unit
        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;

        // Number of days in a non-leap year
        private const int DaysPerYear = 365;

        // Number of days in 4 years
        private const int DaysPer4Years = DaysPerYear * 4 + 1; // 1461

        // Number of days in 100 years
        private const int DaysPer100Years = DaysPer4Years * 25 - 1; // 36524

        // Number of days in 400 years
        private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097

        // Number of days from 1/1/0001 to 12/30/1899
        private const int DaysTo1899 = DaysPer400Years * 4 + DaysPer100Years * 3 - 367;

        private const long DoubleDateOffset = DaysTo1899 * TicksPerDay;

        // The minimum OA date is 0100/01/01 (Note it's year 100).
        // The maximum OA date is 9999/12/31
        private const long OADateMinAsTicks = (DaysPer100Years - DaysPerYear) * TicksPerDay;

        // Number of milliseconds per time unit
        private const int MillisPerSecond = 1000;
        private const int MillisPerMinute = MillisPerSecond * 60;
        private const int MillisPerHour = MillisPerMinute * 60;
        private const int MillisPerDay = MillisPerHour * 24;

        /// <summary>
        /// Converts the DateTime instance into an OLE Automation compatible
        /// </summary>
        public static double ToOADate(this DateTime dateTime)
        {
            return TicksToOADate(dateTime.Ticks);
        }

        private static double TicksToOADate(long value)
        {
            if (value == 0)
                return 0.0;  // Returns OleAut's zero'ed date value.
            if (value < TicksPerDay) // This is a fix for VB. They want the default day to be 1/1/0001 rathar then 12/30/1899.
                value += DoubleDateOffset; // We could have moved this fix down but we would like to keep the bounds check.
            if (value < OADateMinAsTicks)
                throw new OverflowException();
            // Currently, our max date == OA's max date (12/31/9999), so we don't
            // need an overflow check in that direction.
            long millis = (value - DoubleDateOffset) / TicksPerMillisecond;
            if (millis < 0)
            {
                long frac = millis % MillisPerDay;
                if (frac != 0) millis -= (MillisPerDay + frac) * 2;
            }
            return (double)millis / MillisPerDay;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime FromOADate(double d)
        {
#if COREFX
            return new DateTime(DoubleDateToTicks(d), DateTimeKind.Unspecified);
#else
            return System.DateTime.FromOADate(d);
#endif
        }

        private static long DoubleDateToTicks(double value)
        {
            if (value >= 2958466.0 || value <= -657435.0)
            {
                throw new ArgumentException();
            }
            long num = (long)(value * 86400000.0 + ((value >= 0.0) ? 0.5 : -0.5));
            if (num < 0L)
            {
                long expr_5C = num;
                num = expr_5C - expr_5C % 86400000L * 2L;
            }
            num += 59926435200000L;
            if (num < 0L || num >= 315537897600000L)
            {
                throw new ArgumentException();
            }
            return num * 10000L;
        }
    }
}