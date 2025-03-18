using Lib.Common.Attributes;
using Lib.Common.Server.Interfaces;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Shared.Assets;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Assets;

[Mnemonic("prPn")]
public class BO_PROPNEW : IEventHandler<MSG_PROPNEW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState userState ||
            ((IServerApp)userState.App).ServerSessionState is not IServerSessionState serverState ||
            @event is not ProtocolEventParams { Request: MSG_PROPNEW inboundPacket } @params ||
            !serverState.Rooms.TryGetValue(userState.RoomId, out var room)) return null;

        LoggerHub.Current.Debug(nameof(BO_PROPNEW) + $"[{@params.SourceID}]]: {inboundPacket.PropSpec.Id}, {inboundPacket.PropSpec.Crc}");

        room.LooseProps.Add(new LoosePropRec
        {
            AssetSpec = inboundPacket.PropSpec,
            Loc = inboundPacket.Pos,
        });

        return null;
    }
}