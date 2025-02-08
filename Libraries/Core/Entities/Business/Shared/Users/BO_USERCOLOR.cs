using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [Mnemonic("usrC")]
    public partial class BO_USERCOLOR : IIntegrationEventHandler<MSG_USERCOLOR>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}