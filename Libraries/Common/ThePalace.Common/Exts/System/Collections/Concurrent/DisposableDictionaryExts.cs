using ThePalace.Common.Factories;

namespace System.Collections.Concurrent;

public static class DisposableDictionaryExts
{
    //static DisposableDictionaryExts() { }

    public static IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionary<TKey, TValue>(
        this DisposableDictionary<TKey, TValue> values)
        where TValue : IDisposable
    {
        return values;
    }

    public static TValue GetValue<TKey, TValue>(this DisposableDictionary<TKey, TValue> values, TKey key)
        where TValue : IDisposable
    {
        return values != null && values.ContainsKey(key) ? values[key] : default;
    }

    public static TValue GetValueLocked<TKey, TValue>(this DisposableDictionary<TKey, TValue> values, TKey key)
        where TValue : IDisposable
    {
        using (var @lock = LockContext.GetLock(values))
        {
            return values != null && values.ContainsKey(key) ? values[key] : default;
        }
    }

    public static void Remove<TKey, TValue>(this DisposableDictionary<TKey, TValue> values, TKey key)
        where TValue : IDisposable
    {
        values.Remove(key, out var value);
    }

    public static class Types
    {
        public static readonly Type DisposableDictionaryGeneric = typeof(DisposableDictionary<,>);
    }
}