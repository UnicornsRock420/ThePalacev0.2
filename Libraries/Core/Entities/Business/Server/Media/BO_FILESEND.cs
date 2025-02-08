using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Server.Media;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Media
{
    [Mnemonic("sFil")]
    public partial class BO_FILESEND : IProtocolHandler<MSG_FILESEND>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}