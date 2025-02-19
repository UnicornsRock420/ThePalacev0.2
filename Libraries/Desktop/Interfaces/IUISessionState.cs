using System.Collections.Concurrent;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Desktop.Interfaces
{
    public interface IUISessionState : ISessionState
    {
        ConcurrentDictionary<string, object> Extended { get; }
    }
}