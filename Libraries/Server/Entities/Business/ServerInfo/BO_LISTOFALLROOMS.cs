using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Client.ServerInfo;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.ServerInfo;

[Mnemonic("rLst")]
public class BO_LISTOFALLROOMS : IEventHandler<MSG_LISTOFALLROOMS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_LISTOFALLROOMS inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_LISTOFALLROOMS) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_LISTOFALLROOMS));

        return null;
    }
}