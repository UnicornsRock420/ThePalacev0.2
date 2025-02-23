using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Shared.Assets
{
    [Mnemonic("dPrp")]
    public partial class BO_PROPDEL : IEventHandler<MSG_PROPDEL>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}