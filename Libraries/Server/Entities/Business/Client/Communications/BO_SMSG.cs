using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Communications;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Client.Communications
{
    [Mnemonic("smsg")]
    public partial class BO_SMSG : IEventHandler<MSG_SMSG>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}