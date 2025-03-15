using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Users;

[Mnemonic("eprs")]
public class BO_USEREXIT : IEventHandler<MSG_USEREXIT>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}