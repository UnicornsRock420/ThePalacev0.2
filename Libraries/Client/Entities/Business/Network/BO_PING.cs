using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Shared.Network;
using Lib.Core.Helpers.Network;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.Network;

[Mnemonic("ping")]
public class BO_PING : IEventHandler<MSG_PING>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams @params) return null;

        LoggerHub.Current.Debug(nameof(BO_PING) + $"[{@params.SourceID}]: {@params.RefNum}");
        
        sessionState.Send(
            sessionState.UserId,
            new MSG_PONG());

        return null;
    }
}