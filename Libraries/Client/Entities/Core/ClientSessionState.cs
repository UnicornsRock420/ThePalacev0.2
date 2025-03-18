using System.Collections;
using System.Collections.Concurrent;
using System.Net.Sockets;
using Lib.Common.Client.Interfaces;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Entities.Shared.ServerInfo;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Interfaces.Core;
using Lib.Network.Interfaces;

namespace Lib.Common.Client.Entities.Core;

public class ClientSessionState : Disposable, IClientSessionState
{
    public override void Dispose()
    {
        if (IsDisposed) return;

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    public IApp App { get; set; }
    public Guid Id { get; } = Guid.NewGuid();

    public object? SessionTag { get; set; }
    public object? ScriptTag { get; set; }

    public IConnectionState<Socket>? ConnectionState { get; set; }

    public ConcurrentDictionary<string, object> Extended { get; }

    public int UserId { get; set; }

    public UserDesc? UserDesc { get; set; }
    public RegistrationRec? RegInfo { get; set; }

    public short RoomId { get; set; }
    public RoomDesc RoomInfo { get; set; }
    public ConcurrentDictionary<int, UserDesc> RoomUsers { get; set; }
    
    public string? MediaUrl { get; set; }
    public string? ServerName { get; set; }
    public int ServerPopulation { get; set; }
    public ConcurrentDictionary<short, ListRec> Rooms { get; set; }
    public ConcurrentDictionary<int, ListRec> Users { get; set; }
}