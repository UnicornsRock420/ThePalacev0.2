using ThePalace.Core.Enums;
using ThePalace.Core.Exts;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Helpers.Network;

public static class AsyncPalaceSocket
{
    public static void Send(this ISessionState sessionState, int refNum, IProtocol obj, bool directAccess = false)
    {
        ArgumentNullException.ThrowIfNull(sessionState?.ConnectionState,
            nameof(AsyncPalaceSocket) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(obj, nameof(AsyncPalaceSocket) + "." + nameof(obj));

        (directAccess
                ? (Stream?)sessionState.ConnectionState.NetworkStream
                : (Stream?)sessionState.ConnectionState.BytesSend)
            ?.PalaceSerialize(
                refNum,
                obj,
                opts: SerializerOptions.IncludeHeader);
    }
}