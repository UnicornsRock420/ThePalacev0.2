using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Users;

[Mnemonic("log ")]
public class BO_USERLOG : IEventHandler<MSG_USERLOG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}