using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Rooms
{
    [Mnemonic("sRom")]
    public partial class BO_ROOMSETDESC : IIntegrationEventHandler<MSG_ROOMSETDESC>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}