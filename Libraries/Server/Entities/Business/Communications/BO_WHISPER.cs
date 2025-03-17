using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Communications;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Communications;

[Mnemonic("whis")]
public class BO_WHISPER : IEventHandler<MSG_WHISPER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}