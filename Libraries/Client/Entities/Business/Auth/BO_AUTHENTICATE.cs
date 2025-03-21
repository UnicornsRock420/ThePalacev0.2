using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Client.Auth;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.Auth;

[Mnemonic("auth")]
public class BO_AUTHENTICATE : IEventHandler<MSG_AUTHENTICATE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_AUTHENTICATE inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_AUTHENTICATE) + $"[{@params.SourceID}]: {@params.RefNum}");

        // sessionState.Send(
        //     sessionState.UserId,
        //     new MSG_
        //     {
        //     });

        throw new NotImplementedException(nameof(BO_AUTHENTICATE));

        return null;
    }
}