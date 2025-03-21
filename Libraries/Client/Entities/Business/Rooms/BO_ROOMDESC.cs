using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Server.Rooms;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.Rooms;

[DynamicSize]
[Mnemonic("room")]
public class BO_ROOMDESC : IEventHandler<MSG_ROOMDESC>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ROOMDESC inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_ROOMDESC) + $"[{@params.SourceID}]: {@params.RefNum}");
        
        // sessionState.Send(
        //     sessionState.UserId,
        //     new MSG_
        //     {
        //     });

        throw new NotImplementedException(nameof(BO_ROOMDESC));

        return null;
    }
}