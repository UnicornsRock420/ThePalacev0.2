using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Entities.Business.Shared.Users
{
    [DynamicSize(32, 1)]
    [Mnemonic("usrN")]
    public partial class BO_USERNAME : IEventHandler<MSG_USERNAME>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}