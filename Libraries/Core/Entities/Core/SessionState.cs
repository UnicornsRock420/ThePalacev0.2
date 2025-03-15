using System.Collections;
using System.Collections.Concurrent;
using System.Net.Sockets;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Network.Interfaces;

namespace ThePalace.Core.Entities.Core;

public class SessionState : Disposable, ISessionState
{
    public IApp App { get; set; }
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime? LastActivity { get; set; }
    public IConnectionState<Socket>? ConnectionState { get; set; } = null;

    public uint UserId { get; set; } = 0;
    public UserDesc? UserDesc { get; set; } = null;
    public RegistrationRec? RegInfo { get; set; } = null;
    public object? SessionTag { get; set; } = null;

    public ConcurrentDictionary<string, object> Extended { get; set; }
    public object? ScriptTag { get; set; } = null;

    public RoomDesc RoomInfo { get; set; } = new();
    public ConcurrentDictionary<uint, UserDesc> RoomUsers { get; set; } = new();

    public string? MediaUrl { get; set; } = null;
    public string? ServerName { get; set; } = null;
    public int ServerPopulation { get; set; } = 0;
    public List<ListRec> Rooms { get; set; } = [];
    public List<ListRec> Users { get; set; } = [];
}