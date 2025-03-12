using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Rooms;

[Mnemonic("sRom")]
public class BO_ROOMSETDESC : IEventHandler<MSG_ROOMSETDESC>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}