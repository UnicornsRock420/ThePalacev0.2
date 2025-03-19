using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.Users;

[Mnemonic("log ")]
public class BO_USERLOG : IEventHandler<MSG_USERLOG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERLOG inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERLOG) + $"[{@params.SourceID}]: {@params.RefNum}");
        
        // sessionState.Send(
        //     sessionState.UserId,
        //     new MSG_
        //     {
        //     });

        throw new NotImplementedException(nameof(BO_USERLOG));

        return null;
    }
}