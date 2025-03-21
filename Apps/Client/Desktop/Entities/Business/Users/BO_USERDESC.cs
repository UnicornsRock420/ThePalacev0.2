using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Desktop.Entities.Business.Users;

[Mnemonic("usrD")]
public class BO_USERDESC : IEventHandler<MSG_USERDESC>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERDESC inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERDESC) + $"[{@params.SourceID}]: {@params.RefNum}");

        // sessionState.Send(
        //     sessionState.UserId,
        //     new MSG_
        //     {
        //     });

        throw new NotImplementedException(nameof(BO_USERDESC));

        return null;
    }
}