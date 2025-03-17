using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Media;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Media;

[Mnemonic("sFil")]
public class BO_FILESEND : IEventHandler<MSG_FILESEND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}