using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Network.Shared.Communications;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Communications;

[DynamicSize(258, 256)]
[Mnemonic("xtlk")]
public class BO_XTALK : IEventHandler<MSG_XTALK>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}