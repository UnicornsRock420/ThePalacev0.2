using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [ByteSize(4)]
    [Mnemonic("down")]
    public partial class BO_SERVERDOWN : IProtocolHandler<MSG_SERVERDOWN>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}