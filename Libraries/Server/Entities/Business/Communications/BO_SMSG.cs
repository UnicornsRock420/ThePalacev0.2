using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Communications;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Communications;

[Mnemonic("smsg")]
public class BO_SMSG : IEventHandler<MSG_SMSG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}