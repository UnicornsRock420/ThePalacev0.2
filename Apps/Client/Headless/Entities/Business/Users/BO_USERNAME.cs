using Lib.Common.Attributes.Core;
using Lib.Common.Client.Interfaces;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Headless.Entities.Business.Users;

[DynamicSize(32, 1)]
[Mnemonic("usrN")]
public class BO_USERNAME : IEventHandler<MSG_USERNAME>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERNAME inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERNAME) + $"[{@params.SourceID}]: {@params.RefNum}");

        if (!sessionState.RoomUsers.TryGetValue(@params.RefNum, out var user)) return null;

        user.Name = inboundPacket.Name;

        return null;
    }
}