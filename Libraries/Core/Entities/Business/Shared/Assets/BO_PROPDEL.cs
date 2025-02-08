using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Assets
{
    [Mnemonic("dPrp")]
    public partial class BO_PROPDEL : IIntegrationEventHandler<MSG_PROPDEL>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}