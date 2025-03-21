using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Headless.Entities.Business.Users;

[Mnemonic("uSta")]
public class BO_USERSTATUS : IEventHandler<MSG_USERSTATUS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERSTATUS inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERSTATUS) + $"[{@params.SourceID}]: {@params.RefNum}");
        
        // sessionState.Send(
        //     sessionState.UserId,
        //     new MSG_
        //     {
        //     });

        throw new NotImplementedException(nameof(BO_USERSTATUS));

        return null;
    }
}