using System.Collections.Concurrent;
using ThePalace.Common.Desktop.Entities.Core;
using ThePalace.Common.Desktop.Entities.UI;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Desktop.Factories;

public delegate void HotKeyAction(ISessionState sessionState, Keys keys, object? sender = null);

public class HotKeyManager : SingletonDisposable<HotKeyManager>
{
    ~HotKeyManager()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        _keyBindings.Clear();
        _keyBindings = null;

        base.Dispose();
    }

    private ConcurrentDictionary<Keys, HotKeyBinding> _keyBindings = new();
    public IReadOnlyDictionary<Keys, HotKeyBinding> KeyBindings => _keyBindings.AsReadOnly();

    public void RegisterHotKey(Keys keys, ApiBinding binding, params object[] values)
    {
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
        if (!_keyBindings.ContainsKey(keys))
            _keyBindings.TryRemove(keys, out _);
    }

    public bool Invoke(IUISessionState<IDesktopApp> sessionState, Keys keys, object? sender = null, params object[] values)
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