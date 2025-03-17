using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Communications;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Communications;

[Mnemonic("rmsg")]
public class BO_RMSG : IEventHandler<MSG_RMSG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}