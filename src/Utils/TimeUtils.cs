using System;

namespace NoteBin
{
    public static class TimeUtils
    {
        public static long ToUnixTimeMilliseconds(this DateTime dt)
        {
            return new DateTimeOffset(dt).ToUnixTimeMilliseconds();
        }

        public static DateTime FromUnixTimeMilliseconds(long millis)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(millis).UtcDateTime;
        }
    }
}
