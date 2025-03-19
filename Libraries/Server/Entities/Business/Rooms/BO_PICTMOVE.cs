using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Shared.Rooms;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Rooms;

[Mnemonic("pLoc")]
public class BO_PICTMOVE : IEventHandler<MSG_PICTMOVE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_PICTMOVE inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_PICTMOVE) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_PICTMOVE));

        return null;
    }
}