using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Entities.Business.Shared.Communications
{
    [DynamicSize(258, 256)]
    [Mnemonic("xtlk")]
    public partial class BO_XTALK : IEventHandler<MSG_XTALK>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}