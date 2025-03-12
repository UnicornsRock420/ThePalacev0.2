using System.Collections;
using System.Collections.Concurrent;
using ThePalace.Common.Server.Interfaces;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Network.Interfaces;
using RoomID = short;
using UserID = int;

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

    public IApp<ISessionState> App { get; set; }
    public Guid Id => Guid.NewGuid();
    public DateTime? LastActivity { get; set; } = null;
    public IConnectionState? ConnectionState { get; set; } = null;

    public object? SessionTag { get; set; } = null;
    public object? ScriptTag { get; set; } = null;

    public ConcurrentDictionary<string, object> Extended { get; }

    public string? MediaUrl { get; set; } = null;
    public string? ServerName { get; set; } = null;
    public ConcurrentDictionary<RoomID, RoomDesc> Rooms { get; set; } = [];
    public ConcurrentDictionary<UserID, UserDesc> Users { get; set; } = [];
}