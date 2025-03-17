using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Network.Client.ServerInfo;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.ServerInfo;

[ByteSize(0)]
[Mnemonic("uLst")]
public class BO_LISTOFALLUSERS : IEventHandler<MSG_LISTOFALLUSERS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}