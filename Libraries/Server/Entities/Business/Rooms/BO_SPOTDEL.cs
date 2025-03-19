using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Client.Rooms;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Rooms;

[Mnemonic("opSd")]
public class BO_SPOTDEL : IEventHandler<MSG_SPOTDEL>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_SPOTDEL inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_SPOTDEL) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_SPOTDEL));

        return null;
    }
}