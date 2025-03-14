using ThePalace.Common.Server.Interfaces;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Server.Entities.Business.Assets;

[Mnemonic("dPrp")]
public class BO_PROPDEL : IEventHandler<MSG_PROPDEL>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState userState ||
            userState.App.SessionState is not IServerSessionState serverState ||
            @event is not ProtocolEventParams { Request: MSG_PROPDEL inboundPacket } @params ||
            inboundPacket.PropNum < 0 ||
            !serverState.Rooms.TryGetValue(userState.RoomId, out var room) ||
            inboundPacket.PropNum >= (room.LooseProps?.Count ?? 0)) return null;

        LoggerHub.Current.Debug(nameof(BO_ASSETREGI) + $"[{@params.SourceID}]: ...");

        room.LooseProps.RemoveAt(inboundPacket.PropNum);

        return null;
    }
}