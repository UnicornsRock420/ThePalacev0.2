using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Assets
{
    [Mnemonic("qAst")]
    public partial class BO_ASSETQUERY : IProtocolHandler<MSG_ASSETQUERY>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}