using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [DynamicSize(32, 1)]
    [Mnemonic("usrN")]
    public partial class BO_USERNAME : IIntegrationEventHandler<MSG_USERNAME>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}