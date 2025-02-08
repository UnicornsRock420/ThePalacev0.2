using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Rooms;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Rooms
{
    [DynamicSize]
    [Mnemonic("room")]
    public partial class BO_ROOMDESC : IIntegrationEventHandler<MSG_ROOMDESC>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}