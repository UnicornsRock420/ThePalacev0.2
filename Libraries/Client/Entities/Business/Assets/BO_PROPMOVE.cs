using ThePalace.Common.Client.Interfaces;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Client.Entities.Business.Assets;

[Mnemonic("mPrp")]
public class BO_PROPMOVE : IEventHandler<MSG_PROPMOVE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_PROPMOVE inboundPacket } @params ||
            inboundPacket.PropNum < 0 ||
            inboundPacket.PropNum >= (sessionState.RoomInfo?.LooseProps?.Count ?? 0)) return null;

        LoggerHub.Current.Debug(nameof(BO_PROPMOVE) + $"[{@params.SourceID}]: ...");

        sessionState.RoomInfo.LooseProps[inboundPacket.PropNum].Loc = inboundPacket.Pos;

        return null;
    }
}