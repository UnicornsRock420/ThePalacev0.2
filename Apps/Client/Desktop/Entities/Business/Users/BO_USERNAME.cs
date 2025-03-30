using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Entities.Business.Users;

[DynamicSize(32, 1)]
[Mnemonic("usrN")]
public class BO_USERNAME : IEventHandler<MSG_USERNAME>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientDesktopSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERNAME inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERNAME) + $"[{@params.SourceID}]: {@params.RefNum}");

        if (!sessionState.RoomUsers.TryGetValue(@params.RefNum, out var user)) return null;

        user.Name = inboundPacket.Name;

        sessionState.RefreshScreen(LayerScreenTypes.UserNametag);

        return null;
    }
}