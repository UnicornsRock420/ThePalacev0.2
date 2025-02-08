using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Shared.Assets
{
    [Mnemonic("prPn")]
    public partial class BO_PROPNEW : IEventHandler<MSG_PROPNEW>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}