using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Server.Rooms;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Rooms
{
    [DynamicSize]
    [Mnemonic("room")]
    public partial class BO_ROOMDESC : IProtocolHandler<MSG_ROOMDESC>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}