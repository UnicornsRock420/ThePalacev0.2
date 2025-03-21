using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Client.Network;
using Lib.Core.Entities.Network.Server.Network;
using Lib.Core.Helpers.Network;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.Network;

[Mnemonic("tiyr")]
public class BO_TIYID : IEventHandler<MSG_TIYID>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams @params) return null;

        LoggerHub.Current.Debug(nameof(BO_TIYID) + $"[{@params.SourceID}]: {@params.RefNum}");

        sessionState.ConnectionState.IsLittleEndian = true;
        sessionState.UserId = @params.RefNum;
        
        sessionState.Send(
            sessionState.UserId,
            new MSG_LOGON
            {
                RegInfo = sessionState.RegInfo,
            });

        return null;
    }
}