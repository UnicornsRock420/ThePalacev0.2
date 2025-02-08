using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [Mnemonic("durl")]
    public partial class BO_DISPLAYURL : IIntegrationEventHandler<MSG_DISPLAYURL>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}