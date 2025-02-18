using System.Collections.Concurrent;
using System.Diagnostics;
using ThePalace.Common.Desktop.Entities.Core;
using ThePalace.Common.Desktop.Entities.UI;
using ThePalace.Common.Factories;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Client.Core
{
    public delegate void HotKeyAction(ISessionState sessionState, Keys keys, object sender = null);

    public partial class HotKeyManager : SingletonDisposable<HotKeyManager>
    {
        private ConcurrentDictionary<Keys, HotKeyBinding> _keyBindings = new();
        public IReadOnlyDictionary<Keys, HotKeyBinding> KeyBindings => _keyBindings.AsReadOnly();

        public HotKeyManager() { }
        ~HotKeyManager() => Dispose(false);

        public override void Dispose()
        {
            if (this.IsDisposed) return;

            _keyBindings.Clear();
            _keyBindings = null;

            base.Dispose();
        }

        public void RegisterHotKey(Keys keys, ApiBinding binding, params object[] values)
        {
            if (!_keyBindings.ContainsKey(keys) &&
                binding != null)
                _keyBindings.TryAdd(keys, new HotKeyBinding
                {
                    Binding = binding,
                    Values = values,
                });
        }

        public void UnregisterHotKey(Keys keys)
        {
            if (!_keyBindings.ContainsKey(keys))
                _keyBindings.TryRemove(keys, out var _);
        }

        public bool Invoke(ISessionState sessionState, Keys keys, object sender = null, params object[] values)
        {
            if (this.IsDisposed) return false;

            if (_keyBindings.ContainsKey(keys))
                try
                {
                    _keyBindings[keys].Binding.Binding(sessionState, new ApiEvent
                    {
                        Keys = keys,
                        Sender = sender,
                        HotKeyState = _keyBindings[keys].Values,
                        EventState = values,
                    });

                    return true;
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine(ex.Message);
#endif
                }

            return false;
        }
    }
}