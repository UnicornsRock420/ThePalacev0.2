using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Entities.Business.Users;

[Mnemonic("rprs")]
public class BO_USERLIST : IEventHandler<MSG_USERLIST>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientDesktopSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERLIST inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERLIST) + $"[{@params.SourceID}]: {@params.RefNum}");

        sessionState.RoomUsers.Clear();
        foreach (var user in inboundPacket.Users)
        {
            sessionState.RoomUsers.TryAdd(user.UserId, new UserDesc(user));
        }

        sessionState.RefreshScreen(
            LayerScreenTypes.UserProp,
            LayerScreenTypes.UserNametag);

        return null;
    }
}