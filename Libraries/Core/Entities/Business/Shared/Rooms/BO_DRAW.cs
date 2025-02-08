using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Rooms
{
    [Mnemonic("draw")]

    public partial class BO_DRAW : IIntegrationEventHandler<MSG_DRAW>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}