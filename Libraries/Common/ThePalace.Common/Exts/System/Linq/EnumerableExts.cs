namespace System;

public static class EnumerableExts
{
    //static EnumerableExts() { }

    public static T Coalesce<T>(this Array input)
    {
        return input.Cast<T>()
            .FirstOrDefault(v => v != null);
    }

    public static T Coalesce<T>(this List<T> input)
    {
        return input.FirstOrDefault(v => v != null);
    }

    public static T Coalesce<T>(this IList<T> input)
    {
        return input.FirstOrDefault(v => v != null);
    }

    public static T Coalesce<T>(this IEnumerable<T> input)
    {
        return input.FirstOrDefault(v => v != null);
    }

    public static class Types
    {
        public static readonly Type Enumerable = typeof(Enumerable);
    }
}