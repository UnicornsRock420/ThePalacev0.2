using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Network;

[Mnemonic("sInf")]
public class BO_EXTENDEDINFO : IEventHandler<MSG_EXTENDEDINFO>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}