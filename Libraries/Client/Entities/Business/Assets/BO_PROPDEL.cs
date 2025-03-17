using Lib.Common.Attributes;
using Lib.Common.Client.Interfaces;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Shared.Assets;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.Assets;

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