﻿using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Factories;
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

    public static IConnectionState CreateConnection(Socket? handler = null, ConnectionManager? instance = null)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(handler));
        //ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        // TODO: Check banlist record(s)

        var result = new ConnectionState
        {
            Socket = handler,
            NetworkStream = new NetworkStream(handler),
            RemoteAddr = new IPEndPoint(handler.GetIPAddress(), handler.GetPort() ?? 0),
        };

        instance?.Register(result);

        return result;
    }

    public static void DropConnection(IConnectionState connectionState) =>
        connectionState?.Socket?.DropConnection();
}