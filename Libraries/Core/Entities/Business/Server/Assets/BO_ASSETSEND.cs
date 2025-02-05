using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Server.Assets;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Assets
{
    [Mnemonic("sAst")]
    public partial class BO_ASSETSEND : IProtocolHandler<MSG_ASSETSEND>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}