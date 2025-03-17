using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Communications;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Communications;

[Mnemonic("talk")]
public class BO_TALK : IEventHandler<MSG_TALK>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}