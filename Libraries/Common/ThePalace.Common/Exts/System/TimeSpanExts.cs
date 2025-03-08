namespace ThePalace.Common.Exts.System;

public static class TimeSpanExts
{
    //static TimeSpanExts() { }

    public static Task GetDelay(this TimeSpan timespan, CancellationToken cancellationToken)
    {
        return Task.Delay(timespan, cancellationToken);
    }

    public static DateTimeOffset ToOffset(this TimeSpan timespan)
    {
        return DateTime.UtcNow.Add(timespan);
    }

    public static class Types
    {
        public static readonly Type TimeSpan = typeof(TimeSpan);
        public static readonly Type TimeSpanArray = typeof(TimeSpan[]);
        public static readonly Type TimeSpanList = typeof(List<TimeSpan>);
    }
}