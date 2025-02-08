using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Rooms
{
    [Mnemonic("opSn")]
    public partial class BO_SPOTNEW : IIntegrationEventHandler<MSG_SPOTNEW>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}