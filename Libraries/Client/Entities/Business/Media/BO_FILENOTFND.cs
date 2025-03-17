using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Media;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Media;

[Mnemonic("fnfe")]
public class BO_FILENOTFND : IEventHandler<MSG_FILENOTFND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}