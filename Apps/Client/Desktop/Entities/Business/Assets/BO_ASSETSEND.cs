using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Server.Assets;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Entities.Business.Assets;

[Mnemonic("sAst")]
public class BO_ASSETSEND : IEventHandler<MSG_ASSETSEND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientDesktopSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ASSETSEND inboundPacket } @params ||
            inboundPacket.AssetRec.AssetSpec.Id == 0) return null;

        LoggerHub.Current.Debug(nameof(BO_ASSETSEND) + $"[{@params.SourceID}]: {inboundPacket.AssetRec.AssetSpec.Id}, {inboundPacket.AssetRec.AssetSpec.Crc}");

        // TODO

        // var assetStream = new AssetStream(inboundPacket.AssetRec);

        sessionState.RefreshScreen(
            LayerScreenTypes.UserProp,
            LayerScreenTypes.LooseProp);

        throw new NotImplementedException(nameof(BO_ASSETSEND));

        return null;
    }
}