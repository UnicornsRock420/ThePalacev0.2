using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Entities.Business.Users;

[Mnemonic("usrF")]
public class BO_USERFACE : IEventHandler<MSG_USERFACE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientDesktopSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERFACE inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERFACE) + $"[{@params.SourceID}]: {@params.RefNum}");

        if (!sessionState.RoomUsers.TryGetValue(@params.RefNum, out var user)) return null;

        user.FaceNbr = inboundPacket.FaceNbr;

        sessionState.RefreshScreen(LayerScreenTypes.UserProp);

        return null;
    }
}