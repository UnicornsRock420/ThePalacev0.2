using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Client.Entities.Business.Shared.Assets
{
    [Mnemonic("mPrp")]
    public partial class BO_PROPMOVE : IEventHandler<MSG_PROPMOVE>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}