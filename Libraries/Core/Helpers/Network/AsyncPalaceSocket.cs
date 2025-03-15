using ThePalace.Core.Enums;
using ThePalace.Core.Exts;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Helpers.Network;

public static class AsyncPalaceSocket
{
    public static void Send<TApp>(this IUserSessionState<TApp> sessionState, int refNum, IProtocol obj, bool directAccess = false)
        where TApp : IApp
    {
        ArgumentNullException.ThrowIfNull(sessionState, nameof(AsyncPalaceSocket) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(obj, nameof(AsyncPalaceSocket) + "." + nameof(obj));

        using (var ms = new MemoryStream())
        {
            ms.PalaceSerialize(
                refNum,
                obj,
                opts: SerializerOptions.IncludeHeader);

            sessionState.ConnectionState.Send(
                ms.ToArray(),
                directAccess: directAccess);
        }
    }
}