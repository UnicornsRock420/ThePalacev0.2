using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("uSta")]
    public partial class BO_USERSTATUS : IProtocolHandler<MSG_USERSTATUS>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}