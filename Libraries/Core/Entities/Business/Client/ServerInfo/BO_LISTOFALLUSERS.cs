using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Client.ServerInfo;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.ServerInfo
{
    [ByteSize(0)]
    [Mnemonic("uLst")]
    public partial class BO_LISTOFALLUSERS : IProtocolHandler<MSG_LISTOFALLUSERS>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}