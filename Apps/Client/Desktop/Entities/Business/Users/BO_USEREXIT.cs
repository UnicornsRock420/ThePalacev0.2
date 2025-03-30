using Lib.Common.Attributes.Core;
using Lib.Common.Client.Interfaces;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Desktop.Entities.Business.Users;

[Mnemonic("eprs")]
public class BO_USEREXIT : IEventHandler<MSG_USEREXIT>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USEREXIT inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USEREXIT) + $"[{@params.SourceID}]: {@params.RefNum}");

        sessionState.RoomUsers.TryRemove(@params.SourceID, out _);

        return null;
    }
}