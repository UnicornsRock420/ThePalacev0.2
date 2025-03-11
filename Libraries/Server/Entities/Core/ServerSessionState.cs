using System.Collections;
using System.Collections.Concurrent;
using ThePalace.Common.Server.Interfaces;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Network.Interfaces;

namespace ThePalace.Common.Server.Entities.Core;

public class ServerSessionState : Disposable, IServerSessionState
{
    ~ServerSessionState()
    {
        Dispose();
    }

    public override void Dispose()
    {
        ConnectionState?.Dispose();
        ConnectionState = null;

        LastActivity = null;

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    public Guid Id => Guid.NewGuid();
    public DateTime? LastActivity { get; set; } = null;
    public IConnectionState? ConnectionState { get; set; } = null;

    public object? SessionTag { get; set; } = null;
    public object? ScriptTag { get; set; } = null;

    public ConcurrentDictionary<string, object> Extended { get; }

    public string? MediaUrl { get; set; } = null;
    public string? ServerName { get; set; } = null;
    public List<RoomDesc> Rooms { get; set; } = [];
    public List<UserDesc> Users { get; set; } = [];
}