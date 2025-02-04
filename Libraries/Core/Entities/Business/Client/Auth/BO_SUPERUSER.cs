using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Client.Auth;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Auth
{
    [Mnemonic("susr")]
    public partial class BO_SUPERUSER : IProtocolHandler<MSG_SUPERUSER>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}