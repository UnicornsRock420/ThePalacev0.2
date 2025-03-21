using Lib.Common.Attributes.Core;
using Lib.Common.Client.Interfaces;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Assets;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Desktop.Entities.Business.Assets;

[Mnemonic("qAst")]
public class BO_ASSETQUERY : IEventHandler<MSG_ASSETQUERY>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ASSETQUERY inboundPacket } @params ||
            inboundPacket.AssetSpec.Id == 0) return null;

        LoggerHub.Current.Debug(nameof(BO_ASSETQUERY) + $"[{@params.SourceID}]: {inboundPacket.AssetSpec.Id}, {inboundPacket.AssetSpec.Crc}");

        // TODO

        // var assetStream = new AssetStream(inboundPacket.AssetSpec);

        throw new NotImplementedException(nameof(BO_ASSETQUERY));

        return null;
    }
}