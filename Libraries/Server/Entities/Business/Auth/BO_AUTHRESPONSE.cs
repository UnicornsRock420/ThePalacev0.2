using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Server.Auth;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Auth;

[Mnemonic("autr")]
public class BO_AUTHRESPONSE : IEventHandler<MSG_AUTHRESPONSE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_AUTHRESPONSE inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_AUTHRESPONSE) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_AUTHRESPONSE));

        return null;
    }
}