using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.ServerInfo;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.ServerInfo
{
    [Mnemonic("rLst")]
    public partial class BO_LISTOFALLROOMS : IIntegrationEventHandler<MSG_LISTOFALLROOMS>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}