using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Client.Auth;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Auth;

[Mnemonic("susr")]
public class BO_SUPERUSER : IEventHandler<MSG_SUPERUSER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_SUPERUSER inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_SUPERUSER) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_SUPERUSER));

        return null;
    }
}