using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Client.Rooms;

[Mnemonic("opSd")]
public partial class BO_SPOTDEL : IEventHandler<MSG_SPOTDEL>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}