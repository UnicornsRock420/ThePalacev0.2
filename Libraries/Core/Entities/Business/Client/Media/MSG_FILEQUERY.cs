using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Media;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Media
{
    [Mnemonic("qFil")]
    public partial class BO_FILEQUERY : IProtocolHandler<MSG_FILEQUERY>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_FILEQUERY request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}