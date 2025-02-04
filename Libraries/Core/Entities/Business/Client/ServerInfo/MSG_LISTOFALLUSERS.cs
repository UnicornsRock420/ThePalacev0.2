using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.ServerInfo;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.ServerInfo
{
    [ByteSize(0)]
    [Mnemonic("uLst")]
    public partial class BO_LISTOFALLUSERS : IProtocolHandler<MSG_LISTOFALLUSERS>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_LISTOFALLUSERS request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}