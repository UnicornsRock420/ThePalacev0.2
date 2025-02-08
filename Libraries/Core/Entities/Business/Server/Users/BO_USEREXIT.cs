using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("eprs")]
    public partial class BO_USEREXIT : IIntegrationEventHandler<MSG_USEREXIT>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}