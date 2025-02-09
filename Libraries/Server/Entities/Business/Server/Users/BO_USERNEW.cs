using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Entities.Business.Server.Users
{
    [Mnemonic("nprs")]
    public partial class BO_USERNEW : IEventHandler<MSG_USERNEW>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}