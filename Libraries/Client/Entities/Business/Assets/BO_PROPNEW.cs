using ThePalace.Common.Client.Interfaces;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Client.Entities.Business.Assets;

[Mnemonic("prPn")]
public class BO_PROPNEW : IEventHandler<MSG_PROPNEW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_PROPNEW inboundPacket } @params ||
            inboundPacket.PropSpec.Id == 0) return null;

        LoggerHub.Current.Debug(nameof(BO_PROPNEW) + $"[{@params.SourceID}]: {inboundPacket.PropSpec.Id}, {inboundPacket.PropSpec.Crc}");

        sessionState.RoomInfo.LooseProps.Add(new LoosePropRec
        {
            AssetSpec = inboundPacket.PropSpec,
            Loc = inboundPacket.Pos,
        });

        return null;
    }
}