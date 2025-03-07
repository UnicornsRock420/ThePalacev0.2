using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Factories;
using ThePalace.Network.Enums;
using ThePalace.Network.Interfaces;
using ConnectionState = ThePalace.Network.Entities.ConnectionState;

namespace ThePalace.Network.Factories;

public class ConnectionManager : Singleton<ConnectionManager>, IDisposable
{
    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        if (_connectionStates != null)
        {
            using (var @lock = LockContext.GetLock(_connectionStates))
            {
                _connectionStates.Values.ToList().ForEach(s => s?.Socket?.DropConnection());
                _connectionStates.Clear();
            }
            _connectionStates = null;
        }

        GC.SuppressFinalize(this);
    }

    private const uint CONST_INT_UserIDCounterMax = 9999;
    private uint _userIDCounter = 0;
    private bool _isDisposed = false;

    private volatile ConcurrentDictionary<uint, IConnectionState> _connectionStates = new();
    public IReadOnlyDictionary<uint, IConnectionState> ConnectionStates => _connectionStates.AsReadOnly();

    public uint UserID
    {
        get
        {
            if (_userIDCounter >= CONST_INT_UserIDCounterMax)
                _userIDCounter = 0;

            return (uint)++_userIDCounter;
        }
    }

    public uint Register(IConnectionState connectionState)
    {
        if (_isDisposed) return 0;

        var result = UserID;

        using (var @lock = LockContext.GetLock(_connectionStates))
        {
            _connectionStates.TryAdd(result, connectionState);
        }

        return result;
    }

    public uint Register(uint id, IConnectionState connectionState)
    {
        if (_isDisposed) return 0;

        using (var @lock = LockContext.GetLock(_connectionStates))
        {
            _connectionStates.TryAdd(id, connectionState);
        }

        return id;
    }

    public void Unregister(uint id)
    {
        if (_isDisposed) return;

        using (var @lock = LockContext.GetLock(_connectionStates))
        {
            _connectionStates.Remove(id, out var _);
        }
    }

    public void Unregister(IConnectionState connectionState)
    {
        if (_isDisposed) return;

        var id = (uint)0;

        foreach (var state in _connectionStates.ToList())
        {
            if (connectionState.Equals(state) ||
                (connectionState.Socket?.Handle != null &&
                 state.Value.Socket?.Handle != null &&
                 connectionState.Socket?.Handle == state.Value.Socket?.Handle))
            {
                id = state.Key;

                break;
            }
        }

        if (id > 0)
        {
            using (var @lock = LockContext.GetLock(_connectionStates))
            {
                _connectionStates.Remove(id, out var _);
            }
        }
    }

    public static Socket CreateSocket(AddressFamily addressFamily, SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp)
    {
        return new Socket(addressFamily, socketType, protocolType);
    }

    public static NetworkStream CreateNetworkStream(Socket handler)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(ConnectionManager) + "." + nameof(handler));

        return new NetworkStream(handler);
    }

    public static IConnectionState CreateConnectionState(AddressFamily addressFamily, SocketType socketType = SocketType.Stream, IPEndPoint? hostAddr = null, ConnectionManager? instance = null)
    {
        // TODO: Check banlist record(s)

        var handler = CreateSocket(addressFamily, socketType);

        var result = new ConnectionState
        {
            Direction = SocketDirection.Outbound,
            Socket = handler,
            NetworkStream = CreateNetworkStream(handler),
        };

        if (hostAddr != null)
        {
            result.Socket.Connect(result.HostAddr = hostAddr);
        }

        instance ??= ConnectionManager.Current;
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
            RemoteAddr = new IPEndPoint(handler.GetIPAddress(), handler.GetPort() ?? 0),
        };

        instance ??= ConnectionManager.Current;
        instance?.Register(result);

        return result;
    }

    public static void Connect(IConnectionState connectionState, IPEndPoint hostAddr)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(ConnectionManager) + "." + nameof(connectionState));

        connectionState.Socket?.DropConnection();

        connectionState.Direction = SocketDirection.Outbound;

        connectionState.Socket = CreateSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        connectionState.Socket.Connect(hostAddr);

        connectionState.HostAddr = hostAddr;
    }

    public static void DropConnection(IConnectionState connectionState) =>
        connectionState?.Socket?.DropConnection();
}