using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Network.Server.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Network;

[DynamicSize]
[Mnemonic("blow")]
public class BO_BLOWTHRU : IEventHandler<MSG_BLOWTHRU>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}