using Lib.Common.Attributes.Core;
using Lib.Common.Client.Interfaces;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Assets;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Headless.Entities.Business.Assets;

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