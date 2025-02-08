using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [Mnemonic("vers")]

    public partial class BO_VERSION : IIntegrationEventHandler<MSG_VERSION>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}