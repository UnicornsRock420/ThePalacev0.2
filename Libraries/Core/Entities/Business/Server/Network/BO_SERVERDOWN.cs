using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [ByteSize(4)]
    [Mnemonic("down")]
    public partial class BO_SERVERDOWN : IIntegrationEventHandler<MSG_SERVERDOWN>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}