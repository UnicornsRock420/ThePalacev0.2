using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Client.Network
{
    [Mnemonic("bye ")]
    public partial class BO_LOGOFF : IEventHandler<MSG_LOGOFF>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}