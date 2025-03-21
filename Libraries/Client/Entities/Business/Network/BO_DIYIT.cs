using Lib.Common.Attributes.Core;
using Lib.Common.Client.Interfaces;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Server.Network;
using Lib.Core.Exts;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Client.Entities.Business.Network;

[Mnemonic("ryit")]
public class BO_DIYIT : IEventHandler<MSG_DIYIT>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IClientSessionState sessionState ||
            @event is not ProtocolEventParams @params ||
            @params.RefNum == 0) return null;

        @params.RefNum = @params.RefNum.SwapInt32();

        LoggerHub.Current.Debug(nameof(BO_DIYIT) + $"[{@params.SourceID}]: {@params.RefNum}");

        sessionState.ConnectionState.IsLittleEndian = false;
        sessionState.UserId = @params.RefNum;

        return null;
    }
}