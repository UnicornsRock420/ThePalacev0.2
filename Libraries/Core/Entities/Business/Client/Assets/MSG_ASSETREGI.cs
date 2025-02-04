using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Assets
{
    [Mnemonic("rAst")]
    public partial class BO_ASSETREGI : IProtocolHandler<MSG_ASSETREGI>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_ASSETREGI request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}