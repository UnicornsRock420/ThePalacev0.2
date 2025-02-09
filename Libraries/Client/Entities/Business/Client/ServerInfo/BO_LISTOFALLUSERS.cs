using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.ServerInfo;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Client.Entities.Business.Client.ServerInfo
{
    [ByteSize(0)]
    [Mnemonic("uLst")]
    public partial class BO_LISTOFALLUSERS : IEventHandler<MSG_LISTOFALLUSERS>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}