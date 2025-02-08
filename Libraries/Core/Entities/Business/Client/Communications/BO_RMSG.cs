using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("rmsg")]
    public partial class BO_RMSG : IEventHandler<MSG_RMSG>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}