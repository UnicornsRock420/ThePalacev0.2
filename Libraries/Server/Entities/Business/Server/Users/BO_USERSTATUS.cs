using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Server.Users;

[Mnemonic("uSta")]
public partial class BO_USERSTATUS : IEventHandler<MSG_USERSTATUS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}