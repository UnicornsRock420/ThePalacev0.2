using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Entities.Network.Client.Network;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Network;

[DynamicSize]
[Mnemonic("blow")]
public class BO_BLOWTHRU : IEventHandler<MSG_BLOWTHRU>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_BLOWTHRU inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_BLOWTHRU) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_BLOWTHRU));

        return null;
    }
}