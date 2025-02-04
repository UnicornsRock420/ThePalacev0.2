using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Communications;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Communications
{
    [Mnemonic("smsg")]
    public partial class BO_SMSG : IProtocolHandler<MSG_SMSG>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_SMSG request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}