using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Rooms
{
    [Mnemonic("coLs")]
    public partial class BO_SPOTMOVE : IIntegrationEventHandler<MSG_SPOTMOVE>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}