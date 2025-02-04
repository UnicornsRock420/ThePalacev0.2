using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Media;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Media
{
    [Mnemonic("fnfe")]
    public partial class BO_FILENOTFND : IProtocolHandler<MSG_FILENOTFND>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_FILENOTFND request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}