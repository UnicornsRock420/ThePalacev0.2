using Lib.Common.Attributes.Core;
using Lib.Common.Client.Interfaces;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Desktop.Entities.Business.Users;

[Mnemonic("usrC")]
public class BO_USERCOLOR : IEventHandler<MSG_USERCOLOR>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERCOLOR inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERCOLOR) + $"[{@params.SourceID}]: {@params.RefNum}");

        if (!sessionState.RoomUsers.TryGetValue(@params.RefNum, out var user)) return null;
        
        user.ColorNbr = inboundPacket.ColorNbr;
        
        return null;
    }
}