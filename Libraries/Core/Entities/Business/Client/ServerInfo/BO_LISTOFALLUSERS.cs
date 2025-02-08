using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.ServerInfo;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.ServerInfo
{
    [ByteSize(0)]
    [Mnemonic("uLst")]
    public partial class BO_LISTOFALLUSERS : IIntegrationEventHandler<MSG_LISTOFALLUSERS>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}