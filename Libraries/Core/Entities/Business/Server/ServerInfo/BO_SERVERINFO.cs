using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.ServerInfo
{
    [Mnemonic("sinf")]
    public partial class BO_SERVERINFO : IIntegrationEventHandler<MSG_SERVERINFO>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}