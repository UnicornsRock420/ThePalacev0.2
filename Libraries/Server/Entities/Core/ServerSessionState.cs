using System.Collections.Concurrent;
using ThePalace.Common.Factories.System.Collections;
using ThePalace.Common.Server.Interfaces;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Network.Interfaces;

namespace ThePalace.Common.Server.Entities.Core;

public class ServerSessionState : Disposable, IServerSessionState
{
    public void Dispose()
    {
        ConnectionState?.Dispose();
        ConnectionState = null;

        UserDesc = null;
        RegInfo = null;

        LastActivity = null;

        base.Dispose();
    }

    public Guid Id => Guid.NewGuid();
    public DateTime? LastActivity { get; set; } = null;
    public IConnectionState? ConnectionState { get; set; } = null;

    public uint UserId { get; set; } = 0;
    public UserDesc? UserDesc { get; set; } = new();
    public RegistrationRec? RegInfo { get; set; } = new();
    public object? State { get; set; } = null;

    public ConcurrentDictionary<string, object> Extended { get; }
    public object? ScriptState { get; set; } = null;

    public RoomDesc RoomInfo { get; set; } = new();
    public ConcurrentDictionary<uint, UserDesc> RoomUsers { get; set; } = new();

    public string? MediaUrl { get; set; } = null;
    public string? ServerName { get; set; } = null;
    public int ServerPopulation { get; set; } = 0;
    public List<ListRec> ServerRooms { get; set; } = [];
    public List<ListRec> ServerUsers { get; set; } = [];

    ~ServerSessionState()
    {
        Dispose();
    }
}