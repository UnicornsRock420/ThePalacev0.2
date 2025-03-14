using ThePalace.Common.Server.Interfaces;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Server.Entities.Business.Assets;

[Mnemonic("mPrp")]
public class BO_PROPMOVE : IEventHandler<MSG_PROPMOVE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState userState ||
            userState.App.SessionState is not IServerSessionState serverState ||
            @event is not ProtocolEventParams { Request: MSG_PROPMOVE inboundPacket } @params ||
            inboundPacket.PropNum < 0 ||
            !serverState.Rooms.TryGetValue(userState.RoomId, out var room) ||
            inboundPacket.PropNum >= (room.LooseProps?.Count ?? 0)) return null;

        LoggerHub.Current.Debug(nameof(BO_PROPMOVE) + $"[{@params.SourceID}]: ...");

        room.LooseProps[inboundPacket.PropNum].Loc = inboundPacket.Pos;

        return null;
    }
}