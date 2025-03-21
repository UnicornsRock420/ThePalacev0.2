using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Client.Rooms;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Rooms;

[Mnemonic("nRom")]
public class BO_ROOMNEW : IEventHandler<MSG_ROOMNEW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ROOMNEW inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_ROOMNEW) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_ROOMNEW));

        return null;
    }
}