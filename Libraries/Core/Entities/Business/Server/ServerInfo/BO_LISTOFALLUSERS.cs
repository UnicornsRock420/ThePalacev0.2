using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.ServerInfo
{
    [Mnemonic("uLst")]
    public partial class BO_LISTOFALLUSERS : IProtocolHandler<MSG_LISTOFALLUSERS>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}