using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Shared.Rooms;

[Mnemonic("sSta")]
public partial class BO_SPOTSTATE : IEventHandler<MSG_SPOTSTATE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}