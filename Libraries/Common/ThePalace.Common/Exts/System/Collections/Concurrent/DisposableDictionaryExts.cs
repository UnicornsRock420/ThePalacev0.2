using ThePalace.Common.Factories.Core;
using ThePalace.Common.Factories.System.Collections.Concurrent;

namespace ThePalace.Common.Exts.System.Collections.Concurrent;

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
        return values?.TryGetValue(key, out var value) == true ? value : default;
    }

    public static TValue GetValueLocked<TKey, TValue>(this DisposableDictionary<TKey, TValue> values, TKey key)
        where TValue : IDisposable
    {
        using (var @lock = LockContext.GetLock(values))
        {
            return values?.TryGetValue(key, out var value) == true ? value : default;
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