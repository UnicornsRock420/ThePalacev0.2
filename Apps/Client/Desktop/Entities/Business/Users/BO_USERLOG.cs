using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;
using ThePalace.Client.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Entities.Business.Users;

[Mnemonic("log ")]
public class BO_USERLOG : IEventHandler<MSG_USERLOG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientDesktopSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERLOG inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERLOG) + $"[{@params.SourceID}]: {@params.RefNum}");

        sessionState.ServerPopulation = inboundPacket.NbrUsers;
        
        sessionState.RefreshUI();

        return null;
    }
}