using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Rooms;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Rooms
{
    [Mnemonic("endr")]
    public partial class BO_ROOMDESCEND : IIntegrationEventHandler<MSG_ROOMDESCEND>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}