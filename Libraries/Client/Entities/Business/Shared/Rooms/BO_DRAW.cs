using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Client.Entities.Business.Shared.Rooms
{
    [Mnemonic("draw")]

    public partial class BO_DRAW : IEventHandler<MSG_DRAW>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}