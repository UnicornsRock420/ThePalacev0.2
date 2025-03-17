using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Communications;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Communications;

[Mnemonic("xwis")]
public class BO_XWHISPER : IEventHandler<MSG_XWHISPER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}