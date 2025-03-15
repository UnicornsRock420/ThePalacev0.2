using ThePalace.Common.Client.Interfaces;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Server.Assets;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Client.Entities.Business.Assets;

[Mnemonic("sAst")]
public class BO_ASSETSEND : IEventHandler<MSG_ASSETSEND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState<IApp> sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ASSETSEND inboundPacket } @params ||
            inboundPacket.AssetRec.AssetSpec.Id == 0) return null;

        LoggerHub.Current.Debug(nameof(BO_ASSETSEND) + $"[{@params.SourceID}]: {inboundPacket.AssetRec.AssetSpec.Id}, {inboundPacket.AssetRec.AssetSpec.Crc}");

        // TODO

        // var assetStream = new AssetStream(inboundPacket.AssetRec);

        throw new NotImplementedException(nameof(BO_ASSETSEND));

        return null;
    }
}