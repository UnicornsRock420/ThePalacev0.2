using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [Mnemonic("usrD")]
    public partial class BO_USERDESC : IIntegrationEventHandler<MSG_USERDESC>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}