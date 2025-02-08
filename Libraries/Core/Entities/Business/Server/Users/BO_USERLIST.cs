using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("rprs")]
    public partial class BO_USERLIST : IEventHandler<MSG_USERLIST>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}