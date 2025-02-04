using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.ServerInfo
{
    [Mnemonic("uLst")]
    public partial class BO_LISTOFALLUSERS : IProtocolHandler<MSG_LISTOFALLUSERS>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_LISTOFALLUSERS request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}