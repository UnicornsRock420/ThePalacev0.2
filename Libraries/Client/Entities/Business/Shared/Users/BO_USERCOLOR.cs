using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Client.Entities.Business.Shared.Users
{
    [Mnemonic("usrC")]
    public partial class BO_USERCOLOR : IEventHandler<MSG_USERCOLOR>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}