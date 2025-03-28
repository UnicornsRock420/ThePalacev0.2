﻿using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Lib.Common.Factories.Core;
using Lib.Network.Enums;
using Lib.Network.Exts.System.Net.Sockets;
using Lib.Network.Interfaces;
using ConnectionState = Lib.Network.Entities.ConnectionState;
using UserID = int;

namespace Lib.Network.Factories;

public class ConnectionManager : Singleton<ConnectionManager>, IDisposable
{
    private const UserID CONST_INT_MaxCounterLimit = 9999;

    private bool IsDisposed { get; set; }
    private UserID _idCounter = 0;
    private volatile ConcurrentDictionary<UserID, IConnectionState<Socket>> _connectionStates = new();
    public IReadOnlyDictionary<UserID, IConnectionState<Socket>> ConnectionStates => _connectionStates.AsReadOnly();

    public UserID GetNextId(UserID counterLimit = CONST_INT_MaxCounterLimit)
    {
        if (_idCounter >= counterLimit)
            _idCounter = 0;

        return ++_idCounter;
    }

    public void Dispose()
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

        GC.SuppressFinalize(this);
    }

    ~ConnectionManager()
    {
        Dispose();
    }

    public UserID Register(IConnectionState<Socket> connectionState)
    {
        if (IsDisposed) return 0;

        var result = GetNextId();

        using (var @lock = LockContext.GetLock(_connectionStates))
        {
            _connectionStates.TryAdd(result, connectionState);
        }

        return result;
    }

    public UserID Register(UserID id, IConnectionState<Socket> connectionState)
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

    public void Unregister(IConnectionState<Socket> connectionState)
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

    public static IConnectionState<Socket> CreateConnectionState(
        AddressFamily addressFamily = AddressFamily.InterNetwork,
        SocketType socketType = SocketType.Stream,
        IPEndPoint? hostAddr = null,
        ConnectionManager? instance = null)
    {
        // TODO: Check banlist record(s)

        var result = new ConnectionState
        {
            Mode = SocketMode.Outbound,
            HostAddr = hostAddr,
            //NetworkStream = CreateNetworkStream(handler),
        };
        result.Socket = result.CreateSocket(addressFamily, socketType);

        if (hostAddr != null) result.Socket.Connect(hostAddr);

        instance ??= Current;
        instance?.Register(result);

        return result;
    }

    public static IConnectionState<Socket> CreateConnectionState(
        Socket? handler = null,
        ConnectionManager? instance = null)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(ConnectionManager) + "." + nameof(handler));

        // TODO: Check banlist record(s)

        var result = new ConnectionState
        {
            Mode = SocketMode.Inbound,
            RemoteAddr = handler.GetIPEndPoint(),
            //NetworkStream = CreateNetworkStream(handler),
        };
        result.Socket = handler;

        instance ??= Current;
        instance?.Register(result);

        return result;
    }
}