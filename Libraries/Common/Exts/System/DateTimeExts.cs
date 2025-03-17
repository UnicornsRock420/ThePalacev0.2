using System.ComponentModel;
using System.Globalization;

namespace System;

public static class DateTimeExts
{
    public static string ToFormat(this DateTime dateTime, DateTimeFormatsEnum format)
    {
        return dateTime.ToString(format.GetDescription(), DateTimeFormatInfo.InvariantInfo);
    }

    public static string ToFormat(this DateTime dateTime, string format)
    {
        return dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);
    }

    public static string ToRfc3339String(this DateTime dateTime)
    {
        return dateTime.ToFormat(DateTimeFormatsEnum.Rfc3339Iso8601);
    }

    public static DateTimeOffset ToOffset(this DateTime dateTime)
    {
        return dateTime;
    }

    public static long UnixTimestamp(this DateTime dateTime)
    {
        return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
    }

    public static DateTime ToDateTime(this int timestamp)
    {
        return DateTime.UnixEpoch.AddSeconds(timestamp);
    }

    public static DateTime ToDateTime(this uint timestamp)
    {
        return DateTime.UnixEpoch.AddSeconds(timestamp);
    }

    public static int ToTimestamp(this DateTime input)
    {
        return (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
    }

    public static int ToTicks(this DateTime input)
    {
        return (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalMilliseconds;
    }

    public static class Types
    {
        public static readonly Type DateTime = typeof(DateTime);
        public static readonly Type DateTimeArray = typeof(DateTime[]);
        public static readonly Type DateTimeList = typeof(List<DateTime>);
    }

    //static DateTimeExts() { }
}