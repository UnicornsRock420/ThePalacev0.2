using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Network;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Network;

[Mnemonic("sInf")]
public class BO_EXTENDEDINFO : IEventHandler<MSG_EXTENDEDINFO>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_EXTENDEDINFO inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_EXTENDEDINFO) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_EXTENDEDINFO));

        return null;
    }
}