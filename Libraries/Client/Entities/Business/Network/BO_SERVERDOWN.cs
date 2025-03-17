using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Network.Server.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Network;

[ByteSize(4)]
[Mnemonic("down")]
public class BO_SERVERDOWN : IEventHandler<MSG_SERVERDOWN>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}