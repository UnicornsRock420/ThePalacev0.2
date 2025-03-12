using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Factories.Core;
using ThePalace.Common.Factories.System;
using ThePalace.Network.Enums;
using ThePalace.Network.Exts.System.Net.Sockets;
using ThePalace.Network.Helpers.Network;
using ThePalace.Network.Interfaces;
using ConnectionState = ThePalace.Network.Entities.ConnectionState;
using UserID = System.Int32;

namespace ThePalace.Network.Factories;

public class ConnectionManager : SingletonDisposable<ConnectionManager>, IDisposable
{
    private const UserID CONST_INT_UserIDCounterMax = 9999;

    private volatile ConcurrentDictionary<UserID, IConnectionState> _connectionStates = new();
    private UserID _userIDCounter;
    public IReadOnlyDictionary<UserID, IConnectionState> ConnectionStates => _connectionStates.AsReadOnly();

    public UserID UserId
    {
        get
        {
            if (_userIDCounter >= CONST_INT_UserIDCounterMax)
                _userIDCounter = 0;

            return ++_userIDCounter;
        }
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        if (_connectionStates != null)
        {
            using (var @lock = LockContext.GetLock(_connectionStates))
            {
                _connectionStates.Values.ToList().ForEach(s => s?.Disconnect());
                _connectionStates.Clear();
            }

            _connectionStates = null;
        }

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    ~ConnectionManager()
    {
        Dispose();
    }

    public UserID Register(IConnectionState connectionState)
    {
        if (IsDisposed) return 0;

        var result = UserId;

        using (var @lock = LockContext.GetLock(_connectionStates))
        {
            _connectionStates.TryAdd(result, connectionState);
        }

        return result;
    }

    public UserID Register(UserID id, IConnectionState connectionState)
    {
        if (IsDisposed) return 0;

        using (var @lock = LockContext.GetLock(_connectionStates))
        {
            _connectionStates.TryAdd(id, connectionState);
        }

        return id;
    }

    public void Unregister(UserID id)
    {
        if (IsDisposed) return;

        using (var @lock = LockContext.GetLock(_connectionStates))
        {
            _connectionStates.Remove(id, out _);
        }
    }

    public void Unregister(IConnectionState connectionState)
    {
        if (IsDisposed) return;

        var id =
            (from state
                    in _connectionStates.ToList()
                where connectionState.Id == state.Value.Id
                select state.Key)
            .FirstOrDefault();

        if (id < 1) return;

        using (var @lock = LockContext.GetLock(_connectionStates))
        {
            _connectionStates.Remove(id, out _);
        }
    }

    public static Socket CreateSocket(AddressFamily addressFamily, SocketType socketType = SocketType.Stream,
        ProtocolType protocolType = ProtocolType.Tcp)
    {
        return new Socket(addressFamily, socketType, protocolType);
    }

    public static NetworkStream CreateNetworkStream(Socket handler)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(ConnectionManager) + "." + nameof(handler));

        return new NetworkStream(handler);
    }

    public static IConnectionState CreateConnectionState(AddressFamily addressFamily,
        SocketType socketType = SocketType.Stream, IPEndPoint? hostAddr = null, ConnectionManager? instance = null)
    {
        // TODO: Check banlist record(s)

        var handler = CreateSocket(addressFamily, socketType);

        var result = new ConnectionState
        {
            Direction = SocketDirection.Outbound,
            Socket = handler,
            NetworkStream = CreateNetworkStream(handler)
        };

        if (hostAddr != null) result.Socket.Connect(result.HostAddr = hostAddr);

        instance ??= Current;
        instance?.Register(result);

        return result;
    }

    public static IConnectionState CreateConnectionState(Socket? handler = null, ConnectionManager? instance = null)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(ConnectionManager) + "." + nameof(handler));

        // TODO: Check banlist record(s)

        var result = new ConnectionState
        {
            Direction = SocketDirection.Inbound,
            Socket = handler,
            NetworkStream = CreateNetworkStream(handler),
            RemoteAddr = new IPEndPoint(handler.GetIPAddress(), handler.GetPort() ?? 0)
        };

        instance ??= Current;
        instance?.Register(result);

        return result;
    }

    public static void Connect(IConnectionState connectionState, IPEndPoint hostAddr)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(ConnectionManager) + "." + nameof(connectionState));

        connectionState.Socket = CreateSocket(AddressFamily.InterNetwork);
        connectionState.Socket.Connect(hostAddr);

        connectionState.NetworkStream = CreateNetworkStream(connectionState.Socket);

        connectionState.Direction = SocketDirection.Outbound;
        connectionState.HostAddr = hostAddr;
    }

    public static void Connect(IConnectionState connectionState, IPAddress ipAddress, int port)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(ConnectionManager) + "." + nameof(connectionState));

        Connect(connectionState, new IPEndPoint(ipAddress, port));
    }

    public static void Connect(IConnectionState connectionState, string hostname, int port)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(ConnectionManager) + "." + nameof(connectionState));

        var ipAddr = hostname.Resolve();
        if (ipAddr != null)
        {
            Connect(connectionState, ipAddr, port);
        }
    }

    public static void Connect(IConnectionState connectionState, Uri url)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(ConnectionManager) + "." + nameof(connectionState));

        Connect(connectionState, url.Host, url.Port);
    }
}