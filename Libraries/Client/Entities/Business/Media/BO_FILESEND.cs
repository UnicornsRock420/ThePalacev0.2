using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Server.Media;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.Media;

[Mnemonic("sFil")]
public class BO_FILESEND : IEventHandler<MSG_FILESEND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_FILESEND inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_FILESEND) + $"[{@params.SourceID}]: {@params.RefNum}");
        
        // sessionState.Send(
        //     sessionState.UserId,
        //     new MSG_
        //     {
        //     });

        throw new NotImplementedException(nameof(BO_FILESEND));

        return null;
    }
}