namespace System.Collections.Generic;

public static class IReadOnlyDictionaryExts
{
    //static IReadOnlyDictionaryExts() { }

    public static TValue GetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> values, TKey key)
    {
        return values?.TryGetValue(key, out var value) == true ? value : default;
    }

    public static class Types
    {
        public static readonly Type IReadOnlyDictionaryGeneric = typeof(IReadOnlyDictionary<,>);
    }
}