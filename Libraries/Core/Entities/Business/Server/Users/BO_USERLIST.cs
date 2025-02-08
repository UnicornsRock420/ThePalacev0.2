using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("rprs")]
    public partial class BO_USERLIST : IIntegrationEventHandler<MSG_USERLIST>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}