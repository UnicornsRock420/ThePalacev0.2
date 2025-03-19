using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Users;

[Mnemonic("usrC")]
public class BO_USERCOLOR : IEventHandler<MSG_USERCOLOR>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERCOLOR inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERCOLOR) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_USERCOLOR));

        return null;
    }
}