using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Client.Entities.Business.Server.Users
{
    [Mnemonic("uSta")]
    public partial class BO_USERSTATUS : IEventHandler<MSG_USERSTATUS>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}