using Lib.Core.Enums;
using Lib.Core.Exts;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Helpers.Network;

public static class AsyncPalaceSocket
{
    public static void Send<TSessionState, TProtocol>(this TSessionState sessionState, int refNum, TProtocol msgObj, bool directAccess = false)
        where TSessionState : class, IUserSessionState
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

#if DEBUG
            var bytes = ms.ToArray();
            sessionState.ConnectionState.Send(
                bytes,
                directAccess: directAccess);
#else
            sessionState.ConnectionState.Send(
                ms.ToArray(),
                directAccess: directAccess);
#endif
        }
    }

    public static void Send<TSessionState>(this TSessionState sessionState, byte[] data, bool directAccess = false)
        where TSessionState : class, IUserSessionState
    {
        ArgumentNullException.ThrowIfNull(sessionState, nameof(AsyncPalaceSocket) + "." + nameof(Send) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(data, nameof(AsyncPalaceSocket) + "." + nameof(Send) + "." + nameof(data));

        sessionState.ConnectionState.Send(
            data,
            directAccess: directAccess);
    }
}