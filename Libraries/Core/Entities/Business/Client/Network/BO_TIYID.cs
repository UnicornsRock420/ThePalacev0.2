using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Network
{
    [Mnemonic("tiyr")]
    public partial class BO_TIYID : IIntegrationEventHandler<MSG_TIYID>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}