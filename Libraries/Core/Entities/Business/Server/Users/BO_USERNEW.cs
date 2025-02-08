using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("nprs")]
    public partial class BO_USERNEW : IIntegrationEventHandler<MSG_USERNEW>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}