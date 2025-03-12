using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Network;

[Mnemonic("ping")]
public class BO_PING : IEventHandler<MSG_PING>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}