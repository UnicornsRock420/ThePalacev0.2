using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Server.Rooms;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.Rooms;

[Mnemonic("endr")]
public class BO_ROOMDESCEND : IEventHandler<MSG_ROOMDESCEND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ROOMDESCEND inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_ROOMDESCEND) + $"[{@params.SourceID}]: {@params.RefNum}");
        
        // sessionState.Send(
        //     sessionState.UserId,
        //     new MSG_
        //     {
        //     });

        throw new NotImplementedException(nameof(BO_ROOMDESCEND));

        return null;
    }
}