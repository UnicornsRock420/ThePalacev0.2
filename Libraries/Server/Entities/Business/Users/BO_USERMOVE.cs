using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Users;

[Mnemonic("uLoc")]
public class BO_USERMOVE : IEventHandler<MSG_USERMOVE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERMOVE inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERMOVE) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_USERMOVE));

        return null;
    }
}