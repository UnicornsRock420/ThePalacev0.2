using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Network.Client.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Users;

[ByteSize(4)]
[Mnemonic("kill")]
public class BO_KILLUSER : IEventHandler<MSG_KILLUSER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}