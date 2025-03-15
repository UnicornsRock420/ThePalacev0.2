using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Rooms;

[Mnemonic("pLoc")]
public class BO_PICTMOVE : IEventHandler<MSG_PICTMOVE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}