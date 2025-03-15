using ThePalace.Core.Enums;
using ThePalace.Core.Exts;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Helpers.Network;

public static class AsyncPalaceSocket
{
    public static void Send<TSessionState, TProtocol>(this TSessionState sessionState, int refNum, TProtocol msgObj, bool directAccess = false)
        where TSessionState : class, IUserSessionState<IApp>
        where TProtocol : class, IProtocol
    {
        ArgumentNullException.ThrowIfNull(sessionState, nameof(AsyncPalaceSocket) + "." + nameof(Send) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(msgObj, nameof(AsyncPalaceSocket) + "." + nameof(Send) + "." + nameof(msgObj));

        using (var ms = new MemoryStream())
        {
            ms.PalaceSerialize(
                refNum,
                msgObj,
                opts: SerializerOptions.IncludeHeader);

            sessionState.ConnectionState.Send(
                ms.ToArray(),
                directAccess: directAccess);
        }
    }

    public static void Send<TApp, TSessionState, TProtocol>(this TSessionState sessionState, int refNum, TProtocol msgObj, bool directAccess = false)
        where TApp : IApp
        where TSessionState : class, IUserSessionState<TApp>
        where TProtocol : class, IProtocol
    {
        ArgumentNullException.ThrowIfNull(sessionState, nameof(AsyncPalaceSocket) + "." + nameof(Send) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(msgObj, nameof(AsyncPalaceSocket) + "." + nameof(Send) + "." + nameof(msgObj));

        using (var ms = new MemoryStream())
        {
            ms.PalaceSerialize(
                refNum,
                msgObj,
                opts: SerializerOptions.IncludeHeader);

            sessionState.ConnectionState.Send(
                ms.ToArray(),
                directAccess: directAccess);
        }
    }

    public static void Send<TSessionState>(this TSessionState sessionState, byte[] data, bool directAccess = false)
        where TSessionState : class, IUserSessionState<IApp>
    {
        ArgumentNullException.ThrowIfNull(sessionState, nameof(AsyncPalaceSocket) + "." + nameof(Send) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(data, nameof(AsyncPalaceSocket) + "." + nameof(Send) + "." + nameof(data));

        sessionState.ConnectionState.Send(
            data,
            directAccess: directAccess);
    }

    public static void Send<TApp, TSessionState>(this TSessionState sessionState, byte[] data, bool directAccess = false)
        where TApp : IApp
        where TSessionState : class, IUserSessionState<TApp>
    {
        ArgumentNullException.ThrowIfNull(sessionState, nameof(AsyncPalaceSocket) + "." + nameof(Send) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(data, nameof(AsyncPalaceSocket) + "." + nameof(Send) + "." + nameof(data));

        sessionState.ConnectionState.Send(
            data,
            directAccess: directAccess);
    }
}