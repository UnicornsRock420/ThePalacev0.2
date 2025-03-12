using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Users;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Users;

[ByteSize(4)]
[Mnemonic("kill")]
public class BO_KILLUSER : IEventHandler<MSG_KILLUSER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}