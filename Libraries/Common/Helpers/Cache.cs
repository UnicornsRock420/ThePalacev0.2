using System.Collections.Concurrent;
using System.Runtime.Caching;
using Lib.Common.Factories.Core;

namespace Lib.Common.Helpers;

public static class Cache
{
    private static readonly ConcurrentDictionary<string, object> _memory = new();
    private static readonly ObjectCache _cache = MemoryCache.Default;

    public static void Dispose()
    {
        foreach (var p in _memory?.Values?.ToList() ?? [])
        {
            if (p is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        _memory?.Clear();
    }

    public static bool ContainsKey(string key)
    {
        return _memory.ContainsKey(key);
    }

    public static T GetCache<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key) ||
            !_memory.TryGetValue(key, out var value)) return default(T);

        using (var @lock = LockContext.GetLock(_memory))
        {
            return (T)value ?? default(T);
        }
    }

    public static void SetCache<T>(string key, T value)
    {
        if (!string.IsNullOrWhiteSpace(key) &&
            value != null)
            using (var @lock = LockContext.GetLock(_memory))
            {
                if (_memory.ContainsKey(key))
                    _memory[key] = value;
                else
                    _memory.TryAdd(key, value);
            }
    }

    public static T GetCache<T>(
        string key,
        Func<T> callbackGenerateValue,
        TimeSpan? expiresIn = null,
        CacheItemPriority priority = CacheItemPriority.Default,
        TimeSpan? ifUnusedRemoveIn = null,
        string regionName = null)
        where T : class
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key), nameof(key) + " cannot be null");
        ArgumentNullException.ThrowIfNull(callbackGenerateValue, nameof(callbackGenerateValue) + " cannot be null");

        T value = default(T);

        if (value != null) return value;

        value = callbackGenerateValue();

        if (value == null) return default(T);

        try
        {
            var policy = new CacheItemPolicy
            {
                Priority = priority
            };

            if (expiresIn.HasValue)
                policy.AbsoluteExpiration = DateTime.UtcNow.Add(expiresIn.Value);

            if (ifUnusedRemoveIn.HasValue)
                policy.SlidingExpiration = ifUnusedRemoveIn.Value;

            _cache.Set(new CacheItem(key, value, regionName), policy);
        }
        catch
        {
        }

        return value;
    }

    public static void RemoveKey(string key, string regionName = null)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key), nameof(key) + " cannot be null");
    }

    public static class Types
    {
        public const string ClassName = nameof(Cache);
        public static readonly Type Cache = typeof(Cache);
    }
}