using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [Mnemonic("usrP")]
    public partial class BO_USERPROP : IIntegrationEventHandler<MSG_USERPROP>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}