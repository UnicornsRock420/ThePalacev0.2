using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ThePalace.Common.Factories;

namespace System.Collections.Generic;

public static class DictionaryExts
{
    public static class Types
    {
        public static readonly Type DictionaryGeneric = typeof(Dictionary<,>);
    }

    //static DictionaryExts() { }

    public static IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionary<TKey, TValue>(this Dictionary<TKey, TValue> values) =>
        values;

    public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> values, TKey key) =>
        values.ContainsKey(key) ? values[key] : default;

    public static TValue GetValueLocked<TKey, TValue>(this Dictionary<TKey, TValue> values, TKey key)
    {
        using (var @lock = LockContext.GetLock(values))
        {
            return values.ContainsKey(key) ? values[key] : default;
        }
    }

    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> values, TKey key, TValue value = default(TValue))
        where TValue : notnull
    {
        if (values == null ||
            values.Count < 1) return value;

        ref var _value = ref CollectionsMarshal.GetValueRefOrAddDefault(values, key, out var exists);

        if (exists)
        {
            return _value;
        }

        _value = value;
        return value;
    }

    public static bool TryUpdate<TKey, TValue>(this Dictionary<TKey, TValue> values, TKey key, TValue value = default(TValue))
        where TKey : notnull
    {
        if (values == null ||
            values.Count < 1) return false;

        ref var _value = ref CollectionsMarshal.GetValueRefOrNullRef(values, key);
        if (!Unsafe.IsNullRef(ref _value))
        {
            _value = value;
            return true;
        }

        return false;
    }
}