using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Server.Network
{
    [ByteSize(4)]
    [Mnemonic("down")]
    public partial class BO_SERVERDOWN : IEventHandler<MSG_SERVERDOWN>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}