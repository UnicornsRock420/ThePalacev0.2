using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Users
{
    [ByteSize(4)]
    [Mnemonic("kill")]
    public partial class BO_KILLUSER : IIntegrationEventHandler<MSG_KILLUSER>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}