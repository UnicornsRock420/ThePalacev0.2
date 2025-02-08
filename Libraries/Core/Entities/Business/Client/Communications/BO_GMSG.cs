using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Communications;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Client.Communications
{
    [Mnemonic("gmsg")]
    public partial class BO_GMSG : IEventHandler<MSG_GMSG>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}