using ThePalace.Common.Server.Interfaces;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Factories.Filesystem;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Server.Entities.Business.Assets;

[Mnemonic("rAst")]
public class BO_ASSETREGI : IEventHandler<MSG_ASSETREGI>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        var sessionState = sender as IServerSessionState;
        if (sessionState != null) return null;

        var @params = @event as ProtocolEventParams;
        if (@params != null) return null;

        var inboundPacket = @params.Request as MSG_ASSETREGI;
        if (inboundPacket != null) return null;

        if (inboundPacket.AssetRec.AssetSpec.Id == 0) return null;
        
        LoggerHub.Current.Info(nameof(MSG_ASSETREGI) + $"[{@params.SourceID}]: {inboundPacket.AssetRec.AssetSpec.Id}, {inboundPacket.AssetRec.AssetSpec.Crc}");

        var assetStream = new AssetStream(inboundPacket.AssetRec);
            
        // TODO

        return null;
    }
}