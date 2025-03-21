using System.Collections.Concurrent;
using Lib.Common.Desktop.Entities.Core;
using Lib.Common.Desktop.Entities.UI;
using Lib.Common.Desktop.Interfaces;
using Lib.Core.Interfaces.Core;
using Lib.Logging.Entities;

namespace Lib.Common.Desktop.Singletons;

public delegate void HotKeyAction(ISessionState sessionState, Keys keys, object? sender = null);

public class HotKeyManager : Singleton<HotKeyManager>
{
    ~HotKeyManager()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (IsDisposed) return;

        IsDisposed = true;

        _keyBindings.Clear();
        _keyBindings = null;

        GC.SuppressFinalize(this);
    }

    private bool IsDisposed { get; set; }
    private ConcurrentDictionary<Keys, HotKeyBinding> _keyBindings = new();
    public IReadOnlyDictionary<Keys, HotKeyBinding> KeyBindings => _keyBindings.AsReadOnly();

    public void RegisterHotKey(Keys keys, ApiBinding binding, params object[] values)
    {
        if (IsDisposed) return;

        if (!_keyBindings.ContainsKey(keys) &&
            binding != null)
            _keyBindings.TryAdd(keys, new HotKeyBinding
            {
                ApiBinding = binding,
                Values = values
            });
    }

    public void UnregisterHotKey(Keys keys)
    {
        if (IsDisposed) return;

        if (!_keyBindings.ContainsKey(keys))
            _keyBindings.TryRemove(keys, out _);
    }

    public bool Invoke(IUISessionState sessionState, Keys keys, object? sender = null, params object[] values)
    {
        if (IsDisposed) return false;

        if (!_keyBindings.TryGetValue(keys, out var value)) return false;

        try
        {
            value.ApiBinding.Binding(sessionState, new ApiEvent
            {
                Keys = keys,
                Sender = sender,
                HotKeyState = value.Values,
                EventState = values
            });

            return true;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }

        return false;
    }
}