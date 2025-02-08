using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Media;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Media
{
    [Mnemonic("sFil")]
    public partial class BO_FILESEND : IIntegrationEventHandler<MSG_FILESEND>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}