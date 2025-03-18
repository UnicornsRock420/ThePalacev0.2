using System.Collections;
using System.Collections.Concurrent;
using Lib.Common.Server.Interfaces;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Interfaces.Core;
using RoomID = short;
using UserID = int;

namespace Lib.Common.Server.Entities.Core;

public class ServerSessionState : Disposable, IServerSessionState
{
    ~ServerSessionState()
    {
        Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();

        GC.SuppressFinalize(this);
    }

    public IApp App { get; set; }
    public Guid Id => Guid.NewGuid();

    public object? SessionTag { get; set; } = null;
    public object? ScriptTag { get; set; } = null;

    public ConcurrentDictionary<string, object> Extended { get; }

    public string? MediaUrl { get; set; } = null;
    public string? ServerName { get; set; } = null;
    public ConcurrentDictionary<RoomID, RoomDesc> Rooms { get; set; } = new();
    public ConcurrentDictionary<UserID, IUserSessionState> Users { get; set; } = new();
}