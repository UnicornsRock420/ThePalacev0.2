using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Server.Network
{
    [DynamicSize]
    [Mnemonic("blow")]
    public partial class BO_BLOWTHRU : IEventHandler<MSG_BLOWTHRU>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}