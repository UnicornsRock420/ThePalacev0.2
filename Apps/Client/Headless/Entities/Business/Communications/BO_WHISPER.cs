using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Communications;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace ThePalace.Client.Headless.Entities.Business.Communications;

[Mnemonic("whis")]
public class BO_WHISPER : IEventHandler<MSG_WHISPER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_WHISPER inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_WHISPER) + $"[{@params.SourceID}]: {@params.RefNum}");
        
        // sessionState.Send(
        //     sessionState.UserId,
        //     new MSG_
        //     {
        //     });

        throw new NotImplementedException(nameof(BO_WHISPER));

        return null;
    }
}