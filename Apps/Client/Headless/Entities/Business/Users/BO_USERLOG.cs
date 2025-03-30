using Lib.Common.Attributes.Core;
using Lib.Common.Client.Interfaces;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Headless.Entities.Business.Users;

[Mnemonic("log ")]
public class BO_USERLOG : IEventHandler<MSG_USERLOG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERLOG inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERLOG) + $"[{@params.SourceID}]: {@params.RefNum}");

        sessionState.ServerPopulation = inboundPacket.NbrUsers;

        return null;
    }
}