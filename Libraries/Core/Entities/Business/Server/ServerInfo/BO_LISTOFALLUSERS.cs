using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.ServerInfo
{
    [Mnemonic("uLst")]
    public partial class BO_LISTOFALLUSERS : IIntegrationEventHandler<MSG_LISTOFALLUSERS>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}