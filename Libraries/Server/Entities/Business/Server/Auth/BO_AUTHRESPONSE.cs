using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.Auth;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Server.Auth
{
    [Mnemonic("autr")]
    public partial class BO_AUTHRESPONSE : IEventHandler<MSG_AUTHRESPONSE>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}