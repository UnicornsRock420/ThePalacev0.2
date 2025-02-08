using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Network
{
    [Mnemonic("pong")]
    public partial class BO_PONG : IIntegrationEventHandler<MSG_PONG>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}