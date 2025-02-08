using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Network
{
    [Mnemonic("sInf")]
    public partial class BO_EXTENDEDINFO : IIntegrationEventHandler<MSG_EXTENDEDINFO>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}