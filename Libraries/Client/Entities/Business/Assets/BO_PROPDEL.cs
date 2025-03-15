using ThePalace.Common.Attributes;
using ThePalace.Common.Client.Interfaces;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Client.Entities.Business.Assets;

[Mnemonic("dPrp")]
public class BO_PROPDEL : IEventHandler<MSG_PROPDEL>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState<IApp> sessionState ||
            @event is not ProtocolEventParams { Request: MSG_PROPDEL inboundPacket } @params ||
            inboundPacket.PropNum < 0 ||
            inboundPacket.PropNum >= (sessionState.RoomInfo.LooseProps?.Count ?? 0)) return null;

        LoggerHub.Current.Debug(nameof(BO_PROPDEL) + $"[{@params.SourceID}]: ...");

        sessionState.RoomInfo.LooseProps.RemoveAt(inboundPacket.PropNum);

        return null;
    }
}