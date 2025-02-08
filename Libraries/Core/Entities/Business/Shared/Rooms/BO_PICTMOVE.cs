using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Rooms
{
    [Mnemonic("pLoc")]
    public partial class BO_PICTMOVE : IIntegrationEventHandler<MSG_PICTMOVE>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}