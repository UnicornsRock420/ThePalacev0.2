using Lib.Common.Attributes;
using Lib.Common.Client.Interfaces;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Server.Assets;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Desktop.Entities.Business.Assets;

[Mnemonic("sAst")]
public class BO_ASSETSEND : IEventHandler<MSG_ASSETSEND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ASSETSEND inboundPacket } @params ||
            inboundPacket.AssetRec.AssetSpec.Id == 0) return null;

        LoggerHub.Current.Debug(nameof(BO_ASSETSEND) + $"[{@params.SourceID}]: {inboundPacket.AssetRec.AssetSpec.Id}, {inboundPacket.AssetRec.AssetSpec.Crc}");

        // TODO

        // var assetStream = new AssetStream(inboundPacket.AssetRec);

        throw new NotImplementedException(nameof(BO_ASSETSEND));

        return null;
    }
}