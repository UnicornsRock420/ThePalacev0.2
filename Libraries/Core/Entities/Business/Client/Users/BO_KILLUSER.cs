using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Client.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Users
{
    [ByteSize(4)]
    [Mnemonic("kill")]
    public partial class BO_KILLUSER : IProtocolHandler<MSG_KILLUSER>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}