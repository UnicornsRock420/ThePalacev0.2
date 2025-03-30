using Lib.Common.Attributes.Core;
using Lib.Common.Client.Interfaces;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Desktop.Entities.Business.Users;

[Mnemonic("nprs")]
public class BO_USERNEW : IEventHandler<MSG_USERNEW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERNEW inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERNEW) + $"[{@params.SourceID}]: {@params.RefNum}");

        sessionState.RoomUsers.TryAdd(@params.SourceID, inboundPacket.UserDesc);

        return null;
    }
}