using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Shared.Rooms;

[Mnemonic("pLoc")]
public partial class BO_PICTMOVE : IEventHandler<MSG_PICTMOVE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}