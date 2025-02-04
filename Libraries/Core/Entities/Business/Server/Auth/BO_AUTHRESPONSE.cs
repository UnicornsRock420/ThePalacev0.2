using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Server.Auth;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Auth
{
    [Mnemonic("autr")]
    public partial class BO_AUTHRESPONSE : IProtocolHandler<MSG_AUTHRESPONSE>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}