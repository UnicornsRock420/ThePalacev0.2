using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Rooms;

[Mnemonic("lock")]
public class BO_DOORLOCK : IEventHandler<MSG_DOORLOCK>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}