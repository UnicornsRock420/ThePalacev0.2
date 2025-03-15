using ThePalace.Common.Attributes;
using ThePalace.Common.Client.Interfaces;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Client.Entities.Business.Assets;

[Mnemonic("qAst")]
public class BO_ASSETQUERY : IEventHandler<MSG_ASSETQUERY>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState<IApp> sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ASSETQUERY inboundPacket } @params ||
            inboundPacket.AssetSpec.Id == 0) return null;

        LoggerHub.Current.Debug(nameof(BO_ASSETQUERY) + $"[{@params.SourceID}]: {inboundPacket.AssetSpec.Id}, {inboundPacket.AssetSpec.Crc}");

        // TODO

        // var assetStream = new AssetStream(inboundPacket.AssetSpec);

        throw new NotImplementedException(nameof(BO_ASSETQUERY));

        return null;
    }
}