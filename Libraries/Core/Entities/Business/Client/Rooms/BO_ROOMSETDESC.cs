using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Rooms
{
    [Mnemonic("sRom")]
    public partial class BO_ROOMSETDESC : IProtocolHandler<MSG_ROOMSETDESC>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}