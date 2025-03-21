using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Client.Media;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Media;

[Mnemonic("qFil")]
public class BO_FILEQUERY : IEventHandler<MSG_FILEQUERY>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_FILEQUERY inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_FILEQUERY) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_FILEQUERY));

        return null;
    }
}