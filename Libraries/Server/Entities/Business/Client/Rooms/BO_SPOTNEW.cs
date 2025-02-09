using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Entities.Business.Client.Rooms
{
    [Mnemonic("opSn")]
    public partial class BO_SPOTNEW : IEventHandler<MSG_SPOTNEW>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}