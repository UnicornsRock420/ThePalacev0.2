using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Users;

[Mnemonic("usrC")]
public class BO_USERCOLOR : IEventHandler<MSG_USERCOLOR>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}