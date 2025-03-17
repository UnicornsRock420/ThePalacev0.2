using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Communications;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Communications;

[Mnemonic("gmsg")]
public class BO_GMSG : IEventHandler<MSG_GMSG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}