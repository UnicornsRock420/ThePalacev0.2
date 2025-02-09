using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.ServerInfo;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Entities.Business.Client.ServerInfo
{
    [Mnemonic("rLst")]
    public partial class BO_LISTOFALLROOMS : IEventHandler<MSG_LISTOFALLROOMS>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}