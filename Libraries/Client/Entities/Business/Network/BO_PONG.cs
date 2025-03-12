using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Shared.Network;

[Mnemonic("pong")]
public class BO_PONG : IEventHandler<MSG_PONG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}