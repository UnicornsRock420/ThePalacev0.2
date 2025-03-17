using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Network;

[Mnemonic("sErr")]
public class BO_NAVERROR : IEventHandler<MSG_NAVERROR>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}