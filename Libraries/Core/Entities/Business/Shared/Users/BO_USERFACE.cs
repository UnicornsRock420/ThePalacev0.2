using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [Mnemonic("usrF")]
    public partial class BO_USERFACE : IIntegrationEventHandler<MSG_USERFACE>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}