using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Shared.Rooms;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.Rooms;

[Mnemonic("lock")]
public class BO_DOORLOCK : IEventHandler<MSG_DOORLOCK>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_DOORLOCK inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_DOORLOCK) + $"[{@params.SourceID}]: {@params.RefNum}");
        
        // sessionState.Send(
        //     sessionState.UserId,
        //     new MSG_
        //     {
        //     });

        throw new NotImplementedException(nameof(BO_DOORLOCK));

        return null;
    }
}