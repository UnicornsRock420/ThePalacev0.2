using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Client.Communications;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Communications
{
    [Mnemonic("smsg")]
    public partial class BO_SMSG : IProtocolHandler<MSG_SMSG>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}