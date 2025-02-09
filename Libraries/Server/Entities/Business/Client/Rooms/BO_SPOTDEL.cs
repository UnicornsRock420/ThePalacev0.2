using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Entities.Business.Client.Rooms
{
    [Mnemonic("opSd")]
    public partial class BO_SPOTDEL : IEventHandler<MSG_SPOTDEL>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}