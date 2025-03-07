using System.Collections.Concurrent;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Desktop.Interfaces;

public interface IUISessionState : ISessionState, IDisposable
{
    ConcurrentDictionary<string, object> Extended { get; }

    object? ScriptState { get; set; }
}