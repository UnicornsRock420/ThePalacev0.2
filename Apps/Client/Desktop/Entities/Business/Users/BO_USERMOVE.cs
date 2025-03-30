using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Entities.Business.Users;

[Mnemonic("uLoc")]
public class BO_USERMOVE : IEventHandler<MSG_USERMOVE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientDesktopSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERMOVE inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERMOVE) + $"[{@params.SourceID}]: {@params.RefNum}");

        if (!sessionState.RoomUsers.TryGetValue(@params.RefNum, out var user)) return null;

        user.RoomPos = inboundPacket.RoomPos;

        sessionState.RefreshScreen(
            LayerScreenTypes.UserProp,
            LayerScreenTypes.UserNametag,
            LayerScreenTypes.Messages);

        return null;
    }
}