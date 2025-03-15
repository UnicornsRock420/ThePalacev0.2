using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Rooms;

[Mnemonic("unlo")]
public class BO_DOORUNLOCK : IEventHandler<MSG_DOORUNLOCK>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}