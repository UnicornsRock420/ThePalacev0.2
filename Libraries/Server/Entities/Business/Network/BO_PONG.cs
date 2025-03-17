using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Network;

[Mnemonic("pong")]
public class BO_PONG : IEventHandler<MSG_PONG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}