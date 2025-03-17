using System.Collections.Concurrent;
using System.Net.Sockets;
using Lib.Common.Desktop.Interfaces;
using Lib.Core.Entities.Core;
using Lib.Core.Entities.Shared.Users;
using Lib.Network.Interfaces;

namespace Lib.Common.Desktop.Entities.Core;

public class UISessionState : SessionState, IUISessionState<IDesktopApp>
{
    public ConcurrentDictionary<string, object> Extended { get; set; } = new();

    public IDesktopApp App { get; set; }
    public Guid Id => Guid.NewGuid();

    public uint UserId { get; set; }
    public DateTime? LastActivity { get; set; }

    public IConnectionState<Socket> ConnectionState { get; set; }
    public UserDesc UserDesc { get; set; } = new();
    public RegistrationRec RegInfo { get; set; } = new();

    public object? ScriptTag { get; set; }
}