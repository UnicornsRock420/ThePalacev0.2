using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.ServerInfo
{
    [DynamicSize]
    [Mnemonic("rLst")]
    public partial class BO_LISTOFALLROOMS : IProtocolHandler<MSG_LISTOFALLROOMS>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_LISTOFALLROOMS request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}