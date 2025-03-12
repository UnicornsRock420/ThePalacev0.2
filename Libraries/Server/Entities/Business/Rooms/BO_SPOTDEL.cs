using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Rooms;

[Mnemonic("opSd")]
public class BO_SPOTDEL : IEventHandler<MSG_SPOTDEL>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}