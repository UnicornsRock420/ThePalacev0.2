using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Communications
{
    [Mnemonic("xwis")]
    public partial class BO_XWHISPER : IProtocolHandler<MSG_XWHISPER>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}