using Lib.Common.Factories.Core;

namespace System.Collections.Concurrent;

public static class ConcurrentDictionaryExts
{
    //static ConcurrentDictionaryExts() { }

    public static IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionary<TKey, TValue>(
        this ConcurrentDictionary<TKey, TValue> values)
    {
        return values;
    }

    public static TValue GetValue<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> values, TKey key)
    {
        return values?.TryGetValue(key, out var value) == true ? value : default;
    }

    public static TValue GetValueLocked<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> values, TKey key)
    {
        using (var @lock = LockContext.GetLock(values))
        {
            return values?.TryGetValue(key, out var value) == true ? value : default;
        }
    }

    public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> values, TKey key)
    {
        values.Remove(key, out var value);
    }

    public static class Types
    {
        public static readonly Type ConcurrentDictionaryGeneric = typeof(ConcurrentDictionary<,>);
    }
}