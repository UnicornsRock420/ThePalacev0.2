using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Assets
{
    [Mnemonic("rAst")]
    public partial class BO_ASSETREGI : IProtocolHandler<MSG_ASSETREGI>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}