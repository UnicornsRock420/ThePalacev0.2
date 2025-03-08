using System.Collections.Concurrent;
using ThePalace.Common.Desktop.Entities.Core;

namespace ThePalace.Common.Desktop.Factories;

public class ApiManager : SingletonDisposable<ApiManager>
{
    private ConcurrentDictionary<string, ApiBinding> _apiBindings = new();
    public IReadOnlyDictionary<string, ApiBinding> ApiBindings => _apiBindings;

    ~ApiManager()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        base.Dispose();

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