using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Network;

[Mnemonic("ping")]
public class BO_PING : IEventHandler<MSG_PING>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}