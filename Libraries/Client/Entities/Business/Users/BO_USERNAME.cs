using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Users;

[DynamicSize(32, 1)]
[Mnemonic("usrN")]
public class BO_USERNAME : IEventHandler<MSG_USERNAME>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}