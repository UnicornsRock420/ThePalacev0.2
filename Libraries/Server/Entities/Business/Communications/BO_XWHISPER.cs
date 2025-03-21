using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Communications;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Communications;

[Mnemonic("xwis")]
public class BO_XWHISPER : IEventHandler<MSG_XWHISPER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_XWHISPER inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_XWHISPER) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_XWHISPER));

        return null;
    }
}