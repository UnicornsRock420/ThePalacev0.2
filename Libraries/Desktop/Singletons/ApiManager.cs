using System.Collections.Concurrent;
using Lib.Common.Desktop.Entities.Core;

namespace Lib.Common.Desktop.Singletons;

public class ApiManager : Singleton<ApiManager>, IDisposable
{
    private bool IsDisposed { get; set; }
    private ConcurrentDictionary<string, ApiBinding> _apiBindings = new();
    public IReadOnlyDictionary<string, ApiBinding> ApiBindings => _apiBindings;

    ~ApiManager()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (IsDisposed) return;

        IsDisposed = true;

        _apiBindings.Clear();
        _apiBindings = null;
    }

    public void RegisterApi(string friendlyName, EventHandler binding)
    {
        if (!_apiBindings.ContainsKey(friendlyName) &&
            binding != null)
            _apiBindings.TryAdd(friendlyName, new ApiBinding
            {
                Binding = binding
            });
    }

    public void UnregisterApi(string friendlyName)
    {
        if (!_apiBindings.ContainsKey(friendlyName))
            _apiBindings.TryRemove(friendlyName, out _);
    }
}