using System.Collections.Concurrent;
using ThePalace.Common.Factories.Core;

namespace ThePalace.Common.Exts.System.Collections.Concurrent;

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
        return values != null && values.ContainsKey(key) ? values[key] : default;
    }

    public static TValue GetValueLocked<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> values, TKey key)
    {
        using (var @lock = LockContext.GetLock(values))
        {
            return values != null && values.ContainsKey(key) ? values[key] : default;
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