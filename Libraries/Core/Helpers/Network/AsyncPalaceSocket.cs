﻿using ThePalace.Core.Enums;
using ThePalace.Core.Exts;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Network.Helpers.Network;

namespace ThePalace.Core.Helpers.Network;

public static class AsyncPalaceSocket
{
    public static void Send(this ISessionState sessionState, IProtocol obj, bool directAccess = true)
    {
        ArgumentNullException.ThrowIfNull(sessionState?.ConnectionState,
            nameof(AsyncPalaceSocket) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(obj, nameof(AsyncPalaceSocket) + "." + nameof(obj));

        if (directAccess)
            sessionState.ConnectionState.NetworkStream?.PalaceSerialize((int)sessionState.UserId, obj,
                SerializerOptions.IncludeHeader);
        else
            using (var ms = new MemoryStream())
            {
                ms.PalaceSerialize((int)sessionState.UserId, obj, SerializerOptions.IncludeHeader);

                if (ms.Length > 0)
                    sessionState.ConnectionState.Send(ms.GetBuffer());
            }
    }
}