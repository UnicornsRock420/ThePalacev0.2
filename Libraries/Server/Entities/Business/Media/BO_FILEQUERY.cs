using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Media;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Media;

[Mnemonic("qFil")]
public class BO_FILEQUERY : IEventHandler<MSG_FILEQUERY>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}