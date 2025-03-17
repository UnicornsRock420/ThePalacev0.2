using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Network;

[Mnemonic("durl")]
public class BO_DISPLAYURL : IEventHandler<MSG_DISPLAYURL>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}