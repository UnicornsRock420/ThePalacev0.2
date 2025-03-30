using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Assets;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Entities.Business.Assets;

[Mnemonic("prPn")]
public class BO_PROPNEW : IEventHandler<MSG_PROPNEW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientDesktopSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_PROPNEW inboundPacket } @params ||
            inboundPacket.PropSpec.Id == 0) return null;

        LoggerHub.Current.Debug(nameof(BO_PROPNEW) + $"[{@params.SourceID}]: {inboundPacket.PropSpec.Id}, {inboundPacket.PropSpec.Crc}");

        sessionState.RoomInfo.LooseProps.Add(new LoosePropRec
        {
            AssetSpec = inboundPacket.PropSpec,
            Loc = inboundPacket.Pos,
        });

        sessionState.RefreshScreen(LayerScreenTypes.LooseProp);

        return null;
    }
}