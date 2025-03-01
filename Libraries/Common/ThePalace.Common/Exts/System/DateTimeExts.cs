using System.ComponentModel;
using System.Globalization;

namespace System;

public static class DateTimeExts
{
    public static class Types
    {
        public static readonly Type DateTime = typeof(DateTime);
        public static readonly Type DateTimeArray = typeof(DateTime[]);
        public static readonly Type DateTimeList = typeof(List<DateTime>);
    }

    public static string ToFormat(this DateTime dateTime, DateTimeFormatsEnum format) =>
        dateTime.ToString(format.GetDescription(), DateTimeFormatInfo.InvariantInfo);
    public static string ToFormat(this DateTime dateTime, string format) =>
        dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);
    public static string ToRfc3339String(this DateTime dateTime) =>
        dateTime.ToFormat(DateTimeFormatsEnum.Rfc3339Iso8601);
    public static DateTimeOffset ToOffset(this DateTime dateTime) =>
        (DateTimeOffset)dateTime;
    public static long UnixTimestamp(this DateTime dateTime) =>
        ((DateTimeOffset)dateTime).ToUnixTimeSeconds();

    public static DateTime ToDateTime(this int timestamp) =>
        DateTime.UnixEpoch.AddSeconds(timestamp);
    public static DateTime ToDateTime(this uint timestamp) =>
        DateTime.UnixEpoch.AddSeconds(timestamp);
    public static int ToTimestamp(this DateTime input) =>
        (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
    public static int ToTicks(this DateTime input) =>
        (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalMilliseconds;

    //static DateTimeExts() { }
}