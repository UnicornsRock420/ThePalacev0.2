using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Client.Users;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Users;

[ByteSize(4)]
[Mnemonic("kill")]
public class BO_KILLUSER : IEventHandler<MSG_KILLUSER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_KILLUSER inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_KILLUSER) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_KILLUSER));

        return null;
    }
}