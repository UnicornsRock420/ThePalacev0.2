using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Server.ServerInfo;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.ServerInfo;

[Mnemonic("uLst")]
public class BO_LISTOFALLUSERS : IEventHandler<MSG_LISTOFALLUSERS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_LISTOFALLUSERS inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_LISTOFALLUSERS) + $"[{@params.SourceID}]: {@params.RefNum}");
        
        // sessionState.Send(
        //     sessionState.UserId,
        //     new MSG_
        //     {
        //     });

        throw new NotImplementedException(nameof(BO_LISTOFALLUSERS));

        return null;
    }
}