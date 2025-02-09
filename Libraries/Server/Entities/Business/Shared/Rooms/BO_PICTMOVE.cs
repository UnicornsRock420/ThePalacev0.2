using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Entities.Business.Shared.Rooms
{
    [Mnemonic("pLoc")]
    public partial class BO_PICTMOVE : IEventHandler<MSG_PICTMOVE>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}