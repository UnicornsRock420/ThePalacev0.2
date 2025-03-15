using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Rooms;

[Mnemonic("draw")]
public class BO_DRAW : IEventHandler<MSG_DRAW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}