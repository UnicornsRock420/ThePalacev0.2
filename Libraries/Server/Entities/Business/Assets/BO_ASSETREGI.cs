using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Client.Assets;
using Lib.Core.Factories.IO;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Assets;

[Mnemonic("rAst")]
public class BO_ASSETREGI : IEventHandler<MSG_ASSETREGI>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ASSETREGI inboundPacket } @params ||
            inboundPacket.AssetRec.AssetSpec.Id == 0) return null;

        LoggerHub.Current.Debug(nameof(BO_ASSETREGI) + $"[{@params.SourceID}]: {inboundPacket.AssetRec.AssetSpec.Id}, {inboundPacket.AssetRec.AssetSpec.Crc}");

        // TODO
            
        var assetStream = new AssetStream(inboundPacket.AssetRec);

        throw new NotImplementedException(nameof(BO_ASSETREGI));

        return null;
    }
}