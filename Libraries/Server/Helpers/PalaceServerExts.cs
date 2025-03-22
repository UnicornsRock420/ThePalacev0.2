using Lib.Common.Server.Interfaces;
using Lib.Core.Enums;
using Lib.Core.Exts;
using Lib.Core.Helpers.Network;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.Network;

namespace Lib.Common.Server.Helpers;

public static class PalaceServerExts
{
    private static byte[]? _Serialize(this IProtocol msgObj, int refNum)
    {
        using (var ms = new MemoryStream())
        {
            ms.PalaceSerialize(
                refNum,
                msgObj,
                opts: SerializerOptions.IncludeHeader);

            return ms.ToArray();
        }
    }

    private static void _Send(this List<IUserSessionState> users, byte[] msgBytes, bool directAccess = false)
    {
        if ((users?.Count ?? 0) < 1 ||
            (msgBytes?.Length ?? 0) < 1) return;

        foreach (var user in users)
            user.Send(msgBytes, directAccess);
    }

    public static void UserSend<TServerSessionState, TProtocol>(this TServerSessionState sessionState, int userID, int refNum, TProtocol msgObj, bool directAccess = false)
        where TServerSessionState : class, IServerSessionState
        where TProtocol : class, IProtocol
    {
        ArgumentNullException.ThrowIfNull(sessionState, nameof(PalaceServerExts) + "." + nameof(UserSend) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(msgObj, nameof(PalaceServerExts) + "." + nameof(UserSend) + "." + nameof(msgObj));

        var bytes = msgObj._Serialize(refNum);
        if ((bytes?.Length ?? 0) < 1) return;

        var user = sessionState.Users.Values
            .Where(u => u.UserId == userID)
            .ToList();
        user._Send(bytes, directAccess);
    }

    public static void RoomSend<TServerSessionState, TProtocol>(this TServerSessionState sessionState, short roomID, int refNum, TProtocol msgObj, bool directAccess = false)
        where TServerSessionState : class, IServerSessionState
        where TProtocol : class, IProtocol
    {
        ArgumentNullException.ThrowIfNull(sessionState, nameof(PalaceServerExts) + "." + nameof(RoomSend) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(msgObj, nameof(PalaceServerExts) + "." + nameof(RoomSend) + "." + nameof(msgObj));

        var bytes = msgObj._Serialize(refNum);
        if ((bytes?.Length ?? 0) < 1) return;

        var users = sessionState.Users.Values
            .Where(u => u.RoomId == roomID)
            .ToList();
        users._Send(bytes, directAccess);
    }

    public static void ServerSend<TServerSessionState, TProtocol>(this TServerSessionState sessionState, int refNum, TProtocol msgObj, bool directAccess = false)
        where TServerSessionState : class, IServerSessionState
        where TProtocol : class, IProtocol
    {
        ArgumentNullException.ThrowIfNull(sessionState, nameof(PalaceServerExts) + "." + nameof(ServerSend) + "." + nameof(sessionState));
        ArgumentNullException.ThrowIfNull(msgObj, nameof(PalaceServerExts) + "." + nameof(ServerSend) + "." + nameof(msgObj));

        var bytes = msgObj._Serialize(refNum);
        if ((bytes?.Length ?? 0) < 1) return;

        var users = sessionState.Users.Values.ToList();
        users._Send(bytes, directAccess);
    }
}