using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Users;

[Mnemonic("usrD")]
public class BO_USERDESC : IEventHandler<MSG_USERDESC>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERDESC inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERDESC) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_USERDESC));

        return null;
    }
}