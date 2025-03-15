using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Factories.IO;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Server.Entities.Business.Assets;

[Mnemonic("rAst")]
public class BO_ASSETREGI : IEventHandler<MSG_ASSETREGI>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState<IApp> sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ASSETREGI inboundPacket } @params ||
            inboundPacket.AssetRec.AssetSpec.Id == 0) return null;

        LoggerHub.Current.Debug(nameof(BO_ASSETREGI) + $"[{@params.SourceID}]: {inboundPacket.AssetRec.AssetSpec.Id}, {inboundPacket.AssetRec.AssetSpec.Crc}");

        // TODO
            
        var assetStream = new AssetStream(inboundPacket.AssetRec);

        throw new NotImplementedException(nameof(BO_ASSETREGI));

        return null;
    }
}