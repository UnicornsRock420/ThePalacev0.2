using Lib.Common.Attributes;
using Lib.Common.Server.Interfaces;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Shared.Assets;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Assets;

[Mnemonic("dPrp")]
public class BO_PROPDEL : IEventHandler<MSG_PROPDEL>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState userState ||
            ((IServerApp)userState.App).ServerSessionState is not IServerSessionState serverState ||
            @event is not ProtocolEventParams { Request: MSG_PROPDEL inboundPacket } @params ||
            inboundPacket.PropNum < 0 ||
            !serverState.Rooms.TryGetValue(userState.RoomId, out var room) ||
            inboundPacket.PropNum >= (room.LooseProps?.Count ?? 0)) return null;

        LoggerHub.Current.Debug(nameof(BO_ASSETREGI) + $"[{@params.SourceID}]: ...");

        room.LooseProps.RemoveAt(inboundPacket.PropNum);

        return null;
    }
}