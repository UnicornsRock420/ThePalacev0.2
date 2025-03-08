using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Server.Rooms;

[Mnemonic("endr")]
public class BO_ROOMDESCEND : IEventHandler<MSG_ROOMDESCEND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}